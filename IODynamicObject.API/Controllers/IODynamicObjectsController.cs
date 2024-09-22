using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using IODynamicObject.Application.Filters;
using IODynamicObject.Application.Enumeration;
using IODynamicObject.Application.Interfaces.Services;
using IODynamicObject.Application.Types.Customers;
using IODynamicObject.Application.Types.IODynamicObjects;
using IODynamicObject.Application.Types.Orders;
using IODynamicObject.Application.Types.Products;
using IODynamicObject.Application.Validators;
using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Domain.Entities;
using IODynamicObject.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using IODynamicObjectEntity = IODynamicObject.Domain.Entities.IODynamicObject;

namespace IODynamicObject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DynamicObjectController : ControllerBase
    {
        private readonly IIODynamicObjectService _dynamicObjectService;
        private readonly IIODynamicObjectValidator _validator;

        public DynamicObjectController(
            IIODynamicObjectService dynamicObjectService,
            IIODynamicObjectValidator validator)
        {
            _dynamicObjectService = dynamicObjectService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> HandleDynamicObject([FromBody] IODynamicObjectRequest request)
        {
            switch (request.Operation)
            {
                case OperationTypeEnum.Create:
                    return await HandleCreateAsync(request);
                case OperationTypeEnum.Read:
                    return await HandleReadAsync(request);
                case OperationTypeEnum.Update:
                    return await HandleUpdateAsync(request);
                case OperationTypeEnum.Delete:
                    return await HandleDeleteAsync(request);
                default:
                    return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Invalid operation specified."));
            }
        }

        private async Task<IActionResult> HandleCreateAsync(IODynamicObjectRequest request)
        {
            if (request.Data == null)
            {
                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Data is required for create operation."));
            }

            // Deserialize Data into the appropriate model based on Schema
            object dto = DeserializeData(request.Schema, request.Data);

            if (dto == null)
            {
                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Invalid data format."));
            }

            // Perform validation
            var validationErrors = ValidateDto(dto);
            if (validationErrors.Any())
            {
                return BadRequest(new IOResult<List<string>>(IOResultStatusEnum.Error, validationErrors));
            }

            // Serialize DTO back to JSON string for storage
            string jsonData = JsonConvert.SerializeObject(dto);

            // Create dynamic object entity
            var dynamicObject = new IODynamicObjectEntity
            {
                ObjectType = request.Schema.ToString(),
                Data = jsonData,
                CreationDateUtc = DateTime.UtcNow,
                ModificationDateUtc = DateTime.UtcNow
            };

            var result = await _dynamicObjectService.CreateAsync(dynamicObject);

            if (result.Meta.Status == IOResultStatusEnum.Error)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        private async Task<IActionResult> HandleReadAsync(IODynamicObjectRequest request)
        {
            switch (request.Schema)
            {
                case SchemaTypeEnum.Customer:
                    return await ReadObjectsAsync<Customer, CustomerFilterRule>(request);
                case SchemaTypeEnum.Product:
                    return await ReadObjectsAsync<Product, ProductFilter>(request);
                case SchemaTypeEnum.Order:
                    return await ReadObjectsAsync<Order, OrderFilter>(request);
                default:
                    return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Unsupported schema type."));
            }
        }

        private async Task<IActionResult> HandleUpdateAsync(IODynamicObjectRequest request)
        {
            if (request.Data == null)
            {
                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Data is required for update operation."));
            }

            // Deserialize Data into the appropriate model based on Schema
            object dto = DeserializeData(request.Schema, request.Data);

            if (dto == null)
            {
                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Invalid data format."));
            }

            // Perform validation
            var validationErrors = ValidateDto(dto);
            if (validationErrors.Any())
            {
                return BadRequest(new IOResult<List<string>>(IOResultStatusEnum.Error, validationErrors));
            }

            // Serialize DTO back to JSON string for storage
            string jsonData = JsonConvert.SerializeObject(dto);

            // Update dynamic object entity
            var dynamicObject = new IODynamicObjectEntity
            {
                Id = ((IOEntityBase)dto).Id, // Assuming your DTOs inherit from IOEntityBase
                ObjectType = request.Schema.ToString(),
                Data = jsonData,
                ModificationDateUtc = DateTime.UtcNow
            };

            var result = await _dynamicObjectService.UpdateAsync(dynamicObject);

            if (result.Meta.Status == IOResultStatusEnum.Error)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        private async Task<IActionResult> HandleDeleteAsync(IODynamicObjectRequest request)
        {
            if (request.Data == null || !((IDictionary<string, object>)request.Data).ContainsKey("Id"))
            {
                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Id is required for delete operation."));
            }

            long id = Convert.ToInt64(((IDictionary<string, object>)request.Data)["Id"]);

            var result = await _dynamicObjectService.DeleteAsync(id);

            if (result.Meta.Status == IOResultStatusEnum.Error)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        private object DeserializeData(SchemaTypeEnum schema, dynamic data)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);
                switch (schema)
                {
                    case SchemaTypeEnum.Customer:
                        return JsonConvert.DeserializeObject<Customer>(jsonData);
                    case SchemaTypeEnum.Product:
                        return JsonConvert.DeserializeObject<Product>(jsonData);
                    case SchemaTypeEnum.Order:
                        return JsonConvert.DeserializeObject<Order>(jsonData);
                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        private List<string> ValidateDto(object dto)
        {
            var errors = new List<string>();
            var context = new ValidationContext(dto, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(dto, context, results, true);
            if (!isValid)
            {
                errors.AddRange(results.Select(r => r.ErrorMessage));
            }
            return errors;
        }

        private async Task<IActionResult> ReadObjectsAsync<TEntity, TFilter>(IODynamicObjectRequest request, IIOFilterRule<TEntity, TFilter> filterRule = null) where TEntity : IOEntityBase where TFilter : class
        {
            var objectType = request.Schema.ToString();
            var dynamicObjectsResult = await _dynamicObjectService.GetByTypeAsync(objectType);

            if (dynamicObjectsResult.Meta.Status == IOResultStatusEnum.Error)
            {
                return BadRequest(dynamicObjectsResult);
            }

            // Deserialize Data fields into list of T
            var dataList = dynamicObjectsResult.Data
                .Select(o => JsonConvert.DeserializeObject<TEntity>(o.Data))
                .AsQueryable();

            // Apply custom filters
            if (filterRule != null && request.Data != null)
            {
                var filters = request.Data as IDictionary<string, object>;
                if (filters != null)
                {
                    dataList = filterRule.ApplyFilters(dataList, filters);
                }
            }

            // Apply pagination and sorting
            int pageNumber = request.PageNumber ?? 1;
            int pageSize = request.PageSize ?? 10;
            string sortBy = request.SortBy ?? "Id";
            string sortOrder = request.SortOrder ?? "asc";

            var filteredResult = IOResultFilter<TEntity>.Create(dataList, pageNumber, pageSize, sortBy, sortOrder);

            return Ok(filteredResult);
        }
    }
}
