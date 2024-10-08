//using System.ComponentModel.DataAnnotations;
//using IODynamicObject.Application.Types.Customers;
//using IODynamicObject.Application.Types.IODynamicObjects;
//using IODynamicObject.Application.Types.Orders;
//using IODynamicObject.Application.Types.Products;
//using IODynamicObject.Application.Validators;
//using IODynamicObject.Core.Metadata.Enumeration;
//using IODynamicObject.Core.Metadata.Models;
//using IODynamicObject.Domain.Entities;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using IODynamicObject.Domain.Enumeration;
//using IODynamicObject.Infrastructure.Services;

//namespace IODynamicObject.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class DynamicObjectController : ControllerBase
//    {
//        private readonly IOCustomerService _customerService;
//        private readonly IIODynamicObjectValidator _validator;

//        public DynamicObjectController(
//            IOCustomerService customerService,
//            IIODynamicObjectValidator validator)
//        {
//            _customerService = customerService;
//            _validator = validator;
//        }

//        [HttpPost]
//        public async Task<IActionResult> HandleDynamicObject([FromBody] IODynamicObjectRequest request)
//        {
//            switch (request.Operation)
//            {
//                case OperationTypeEnum.Create:
//                    return await HandleCreateAsync(request);
//                case OperationTypeEnum.Read:
//                    return await HandleReadAsync(request);
//                case OperationTypeEnum.Update:
//                    return await HandleUpdateAsync(request);
//                case OperationTypeEnum.Delete:
//                    return await HandleDeleteAsync(request);
//                default:
//                    return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Invalid operation specified."));
//            }
//        }

//        private async Task<IActionResult> HandleCreateAsync(IODynamicObjectRequest request)
//        {
//            if (request.Data == null)
//            {
//                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Data is required for create operation."));
//            }

//            // Deserialize Data into the appropriate model based on Schema and assign GUID
//            object dto = Deserialize(request.Schema, request.Data);

//            if (dto == null)
//            {
//                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Invalid data format."));
//            }

//            // Perform validation
//            var validationErrors = ValidateDto(dto);
//            if (validationErrors.Any())
//            {
//                return BadRequest(new IOResult<List<string>>(IOResultStatusEnum.Error, validationErrors));
//            }

//            // Serialize DTO back to JSON string for storage
//            string jsonData = JsonConvert.SerializeObject(dto);

//            // Create dynamic object entity
//            var dynamicObject = new IODynamicObjectEntity
//            {
//                SchemaType = request.Schema,
//                Data = jsonData,
//                CreationDateUtc = DateTime.UtcNow,
//                ModificationDateUtc = DateTime.UtcNow
//            };

//            // Save the dynamic object using the service
//            var result = await _dynamicObjectService.CreateAsync(dynamicObject);

//            if (result.Meta.Status == IOResultStatusEnum.Error)
//            {
//                return BadRequest(result);
//            }

//            return Ok(result);
//        }

//        private object Deserialize(SchemaTypeEnum schemaType, dynamic data)
//        {
//            try
//            {
//                string jsonData = JsonConvert.SerializeObject(data);

//                switch (schemaType)
//                {
//                    case SchemaTypeEnum.Customer:
//                        var customer = JsonConvert.DeserializeObject<IOCustomer>(jsonData);
//                        return customer;

//                    case SchemaTypeEnum.Product:
//                        var product = JsonConvert.DeserializeObject<IOProduct>(jsonData);
//                        return product;

//                    case SchemaTypeEnum.Order:
//                        var order = JsonConvert.DeserializeObject<IOOrder>(jsonData);
//                        return order;

//                    default:
//                        return null;
//                }
//            }
//            catch
//            {
//                return null;
//            }
//        }

//        private async Task<IActionResult> HandleReadAsync(IODynamicObjectRequest request)
//        {
//            dynamic filter = null;
//            if (request.Data != null)
//            {
//                string jsonData = JsonConvert.SerializeObject(request.Data);
//                switch (request.Schema)
//                {
//                    case SchemaTypeEnum.Customer:
//                        filter = JsonConvert.DeserializeObject<CustomerFilter>(jsonData);
//                        break;
//                    case SchemaTypeEnum.Order:
//                        filter = JsonConvert.DeserializeObject<OrderFilter>(jsonData);
//                        break;
//                    case SchemaTypeEnum.Product:
//                        filter = JsonConvert.DeserializeObject<ProductFilter>(jsonData);
//                        break;
//                    default:
//                        return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Unsupported schema type."));
//                }
//            }

//            var result = await _dynamicObjectService.GetByFiltersAsync(request.Schema, filter);

//            if (result.Meta.Status == IOResultStatusEnum.Error)
//            {
//                return BadRequest(result);
//            }

//            return Ok(result);
//        }

//        private async Task<IActionResult> HandleUpdateAsync(IODynamicObjectRequest request)
//        {
//            if (request.Data == null)
//            {
//                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Data is required for update operation."));
//            }

//            // Deserialize Data into the appropriate model based on Schema
//            var dto = DeserializeData(request.Schema, request.Data);

//            if (dto == null)
//            {
//                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Invalid data format."));
//            }

//            // Extract the Guid from the deserialized data
//            var guidProperty = dto.GetType().GetProperty("Guid");
//            if (guidProperty == null)
//            {
//                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Guid not found in the data."));
//            }
//            var guid = (Guid)guidProperty.GetValue(dto);

//            // Retrieve the object from the database using the Guid
//            var existingObjectResult = await _dynamicObjectService.GetByGuidAsync(request.Schema, guid);
//            if (existingObjectResult.Meta.Status == IOResultStatusEnum.Error)
//            {
//                return BadRequest(existingObjectResult);
//            }

//            var existingObject = existingObjectResult.Data;

//            // Perform validation on the new data
//            List<string> validationErrors = ValidateDto(dto);
//            if (validationErrors.Any())
//            {
//                return BadRequest(new IOResult<List<string>>(IOResultStatusEnum.Error, validationErrors));
//            }

//            // Serialize DTO back to JSON string for storage
//            string jsonData = JsonConvert.SerializeObject(dto);

//            // Update dynamic object entity
//            var dynamicObject = new IODynamicObjectEntity
//            {
//                Id = existingObject.Id, // Use the existing DB Id
//                SchemaType = request.Schema,
//                Data = jsonData,
//                ModificationDateUtc = DateTime.UtcNow
//            };

//            var result = await _dynamicObjectService.UpdateAsync(dynamicObject);

//            if (result.Meta.Status == IOResultStatusEnum.Error)
//            {
//                return BadRequest(result);
//            }

//            return Ok(result);
//        }

//        private async Task<IActionResult> HandleDeleteAsync(IODynamicObjectRequest request)
//        {
//            if (request.Data == null)
//            {
//                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Data is required for update operation."));
//            }

//            // Deserialize Data into the appropriate model based on Schema
//            var dto = DeserializeData(request.Schema, request.Data);

//            if (dto == null)
//            {
//                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Invalid data format."));
//            }

//            // Extract the Guid from the deserialized data
//            var guidProperty = dto.GetType().GetProperty("Guid");
//            if (guidProperty == null)
//            {
//                return BadRequest(new IOResult<string>(IOResultStatusEnum.Error, "Guid not found in the data."));
//            }
//            var guid = (Guid)guidProperty.GetValue(dto);

//            // Retrieve the object from the database using the Guid
//            var existingObjectResult = await _dynamicObjectService.GetByGuidAsync(request.Schema, guid);
//            if (existingObjectResult.Meta.Status == IOResultStatusEnum.Error)
//            {
//                return BadRequest(existingObjectResult);
//            }

//            var existingObject = existingObjectResult.Data;

//            // Delete the object using the Id retrieved from the existing object
//            var result = await _dynamicObjectService.DeleteAsync(existingObject.Id);

//            if (result.Meta.Status == IOResultStatusEnum.Error)
//            {
//                return BadRequest(result);
//            }

//            return Ok(result);
//        }


//        private object DeserializeData(SchemaTypeEnum schema, dynamic data)
//        {
//            try
//            {
//                string jsonData = JsonConvert.SerializeObject(data);
//                switch (schema)
//                {
//                    case SchemaTypeEnum.Customer:
//                        return JsonConvert.DeserializeObject<IOCustomer>(jsonData);
//                    case SchemaTypeEnum.Product:
//                        return JsonConvert.DeserializeObject<IOProduct>(jsonData);
//                    case SchemaTypeEnum.Order:
//                        return JsonConvert.DeserializeObject<IOOrder>(jsonData);
//                    default:
//                        return null;
//                }
//            }
//            catch
//            {
//                return null;
//            }
//        }

//        private List<string> ValidateDto(object dto)

//        {
//            var errors = new List<string>();
//            var context = new ValidationContext(dto, null, null);
//            var results = new List<ValidationResult>();
//            bool isValid = Validator.TryValidateObject(dto, context, results, true);
//            if (!isValid)
//            {
//                errors.AddRange(results.Select(r => r.ErrorMessage));
//            }
//            return errors;
//        }
//    }
//}
