using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using IODynamicObject.Application.DTOs.Requests;
using IODynamicObject.Application.DTOs.Responses;
using IODynamicObject.Application.Interfaces.Services;
using IODynamicObject.Application.Validators;
using IODynamicObject.Domain.Entities;
using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;
using Microsoft.AspNetCore.Mvc;
using IODynamicObjectEntity = IODynamicObject.Domain.Entities.IODynamicObject;

namespace IODynamicObject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IODynamicObjectsController : ControllerBase
    {
        private readonly IIODynamicObjectService _dynamicObjectService;
        private readonly IIODynamicObjectValidator _validator;

        public IODynamicObjectsController(IIODynamicObjectService dynamicObjectService, 
                                            IIODynamicObjectValidator validator)
        {
            _dynamicObjectService = dynamicObjectService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDynamicObject([FromBody] IODynamicObjectRequest request)
        {
            var validationErrors = _validator.Validate(request);
            if (validationErrors.Count > 0)
            {
                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, validationErrors[0]));
            }

            var dynamicObject = new IODynamicObjectEntity
            {
                ObjectType = request.ObjectType,
                Data = JsonSerializer.Serialize(request.Data)
            };

            var result = await _dynamicObjectService.CreateAsync(dynamicObject);

            if (result.Meta.Status == IOResultStatusEnum.Error)
            {
                return BadRequest(result.Meta.Messages);
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDynamicObject(long id)
        {
            var result = await _dynamicObjectService.GetByIdAsync(id);

            if (result.Meta.Status == IOResultStatusEnum.Error)
            {
                return NotFound(result.Meta.Messages);
            }

            var response = new IODynamicObjectResponse
            {
                Id = result.Data.Id,
                ObjectType = result.Data.ObjectType,
                Data = JsonSerializer.Deserialize<Dictionary<string, object>>(result.Data.Data),
                CreationDateUtc = result.Data.CreationDateUtc,
                ModificationDateUtc = result.Data.ModificationDateUtc
            };

            return Ok(new IOResult<IODynamicObjectResponse>(result.Meta.Status, response));
        }

        [HttpGet]
        public async Task<IActionResult> GetDynamicObjects(
            [FromQuery] string objectType,
            [FromQuery] Dictionary<string, string> filters,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "Id",
            [FromQuery] string sortOrder = "asc")
        {
            var result = await _dynamicObjectService.GetByTypeAndFiltersAsync(objectType, filters, pageNumber, pageSize, sortBy, sortOrder);

            if (result.Meta.Status == IOResultStatusEnum.Error)
            {
                return BadRequest(new IOResult<string>(result.Meta.Status, result.Meta.Messages));
            }

            var responses = result.Data.Select(obj => new IODynamicObjectResponse
            {
                Id = obj.Id,
                ObjectType = obj.ObjectType,
                Data = JsonSerializer.Deserialize<Dictionary<string, object>>(obj.Data),
                CreationDateUtc = obj.CreationDateUtc,
                ModificationDateUtc = obj.ModificationDateUtc
            }).ToList();

            return Ok(new IOResult<List<IODynamicObjectResponse>>(result.Meta.Status, responses));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDynamicObject(long id, [FromBody] IODynamicObjectRequest request)
        {
            var validationErrors = _validator.Validate(request);
            if (validationErrors.Count > 0)
            {
                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, validationErrors[0]));
            }

            var dynamicObject = new IODynamicObjectEntity
            {
                Id = id,
                ObjectType = request.ObjectType,
                Data = JsonSerializer.Serialize(request.Data)
            };

            var result = await _dynamicObjectService.UpdateAsync(dynamicObject);

            if (result.Meta.Status == IOResultStatusEnum.Error)
            {
                return BadRequest(result.Meta.Messages);
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDynamicObject(long id)
        {
            var result = await _dynamicObjectService.DeleteAsync(id);

            if (result.Meta.Status == IOResultStatusEnum.Error)
            {
                return NotFound(result.Meta.Messages);
            }

            return Ok(result);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] IOOrderRequest request)
        {
            using var transaction = await _dynamicObjectService.BeginTransactionAsync();

            try
            {
                // Validate Order
                var orderValidationErrors = new List<string>();
                if (!request.OrderData.ContainsKey("CustomerId"))
                {
                    orderValidationErrors.Add("Field 'CustomerId' is required for Order.");
                }

                if (request.OrderItems == null || request.OrderItems.Count == 0)
                {
                    orderValidationErrors.Add("At least one OrderItem is required.");
                }

                foreach (var item in request.OrderItems)
                {
                    if (!item.ContainsKey("ProductId") || !item.ContainsKey("Quantity"))
                    {
                        orderValidationErrors.Add("Each OrderItem must contain 'ProductId' and 'Quantity'.");
                        break;
                    }
                }

                if (orderValidationErrors.Count > 0)
                {
                    return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, orderValidationErrors[0]));
                }

                // Create Order
                var order = new IODynamicObjectEntity
                {
                    ObjectType = "Order",
                    Data = JsonSerializer.Serialize(request.OrderData)
                };

                var orderResult = await _dynamicObjectService.CreateAsync(order);

                if (orderResult.Meta.Status == IOResultStatusEnum.Error)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(orderResult.Meta.Messages);
                }

                // Create Order Items
                foreach (var itemData in request.OrderItems)
                {
                    itemData["OrderId"] = orderResult.Data.Id; // Link OrderItem to Order

                    var orderItem = new IODynamicObjectEntity
                    {
                        ObjectType = "OrderItem",
                        Data = JsonSerializer.Serialize(itemData)
                    };

                    var itemResult = await _dynamicObjectService.CreateAsync(orderItem);

                    if (itemResult.Meta.Status == IOResultStatusEnum.Error)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(itemResult.Meta.Messages);
                    }
                }

                await transaction.CommitAsync();

                return Ok(orderResult);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new IOResult<string>(IOResultStatusEnum.Error, ex.Message));
            }
        }
    }
}
