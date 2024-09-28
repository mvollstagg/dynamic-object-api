using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using IODynamicObject.Application.Interfaces.Services;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Domain.Entities;
using Newtonsoft.Json.Linq;
using IODynamicObject.Application.Types.Customers;
using IODynamicObject.Application.Types.Orders;
using IODynamicObject.Application.Types.Products;
using IODynamicObject.Infrastructure.Services;

namespace IODynamicObject.API.Controllers
{
    [ApiController]
    [Route("api/dynamic")]
    public class DynamicController : ControllerBase
    {
        private readonly IOCustomerService _customerService;

        public DynamicController(IOCustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> HandleDynamicRequest([FromBody] JObject requestData)
        {
            try
            {
                // Parse the operation and object type
                string operation = requestData["operation"].ToString().ToLower();
                string objectType = requestData["objectType"].ToString().ToLower();
                long? id = requestData["id"]?.ToObject<long>();

                switch (operation)
                {
                    case "create":
                        return await HandleCreate(objectType, requestData["data"]);

                    case "read":
                        if (id == null) return BadRequest("ID is required for reading.");
                        return await HandleRead(objectType, id.Value);

                    case "update":
                        if (id == null) return BadRequest("ID is required for updating.");
                        return await HandleUpdate(objectType, requestData["data"], id.Value);

                    case "delete":
                        if (id == null) return BadRequest("ID is required for deleting.");
                        return await HandleDelete(objectType, id.Value);

                    default:
                        return BadRequest("Invalid operation.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Helper methods for handling operations
        private async Task<IActionResult> HandleCreate(string objectType, JToken data)
        {
            switch (objectType)
            {
                case "customer":
                    var customer = data.ToObject<IOCustomer>();

                    // Process dynamic objects such as addresses
                    var dynamicObjects = data["dynamicObjects"]?.ToObject<List<IOObject>>();
                    if (dynamicObjects != null)
                    {
                        customer.DynamicObjects = dynamicObjects;
                    }

                    var customerResult = await _customerService.CreateAsync(customer);
                    return Ok(customerResult);

                //case "order":
                //    var order = data.ToObject<IOOrder>();
                //    var orderResult = await _orderService.CreateAsync(order);
                //    return Ok(orderResult);

                //case "product":
                //    var product = data.ToObject<IOProduct>();
                //    var productResult = await _productService.CreateAsync(product);
                //    return Ok(productResult);

                default:
                    return BadRequest("Invalid object type.");
            }
        }

        private async Task<IActionResult> HandleRead(string objectType, long id)
        {
            switch (objectType)
            {
                case "customer":
                    var customerResult = await _customerService.GetByIdAsync(id);
                    var customerDto = MapCustomerToDto(customerResult.Data);
                    return Ok(customerDto);

                //case "order":
                //    var orderResult = await _orderService.GetByIdAsync(id);
                //    return Ok(orderResult);

                //case "product":
                //    var productResult = await _productService.GetByIdAsync(id);
                //    return Ok(productResult);

                default:
                    return BadRequest("Invalid object type.");
            }
        }

        private CustomerModel MapCustomerToDto(IOCustomer customer)
        {
            var customerDto = new CustomerModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                CreationDateUtc = customer.CreationDateUtc,
                ModificationDateUtc = customer.ModificationDateUtc,
                Id = customer.Id,
                DynamicObjects = new List<Dictionary<string, List<Dictionary<string, string>>>>()
            };

            foreach (var dynamicObject in customer.DynamicObjects)
            {
                var dynamicObjectEntry = new Dictionary<string, List<Dictionary<string, string>>>();

                if (dynamicObject.Name == "Address")
                {
                    var addressList = new List<Dictionary<string, string>>();

                    for (int i = 0; i < dynamicObject.Fields.First().Values.Count; i++)
                    {
                        var addressEntry = new Dictionary<string, string>();

                        foreach (var field in dynamicObject.Fields)
                        {
                            addressEntry[field.Name] = field.Values[i].Value;
                        }

                        addressList.Add(addressEntry);
                    }

                    dynamicObjectEntry[dynamicObject.Name] = addressList;
                }
                else if (dynamicObject.Name == "PhoneNumbers")
                {
                    var phoneNumbers = new Dictionary<string, string>();

                    foreach (var field in dynamicObject.Fields)
                    {
                        phoneNumbers[field.Name] = field.Values.First().Value; // Since phone numbers are single values
                    }

                    dynamicObjectEntry[dynamicObject.Name] = new List<Dictionary<string, string>> { phoneNumbers };
                }

                customerDto.DynamicObjects.Add(dynamicObjectEntry);
            }

            return customerDto;
        }

        private async Task<IActionResult> HandleUpdate(string objectType, JToken data, long id)
        {
            switch (objectType)
            {
                case "customer":
                    var customer = data.ToObject<IOCustomer>();
                    customer.Id = id;
                    var customerResult = await _customerService.UpdateAsync(customer);
                    return Ok(customerResult);

                //case "order":
                //    var order = data.ToObject<IOOrder>();
                //    order.Id = id;
                //    var orderResult = await _orderService.UpdateAsync(order);
                //    return Ok(orderResult);

                //case "product":
                //    var product = data.ToObject<IOProduct>();
                //    product.Id = id;
                //    var productResult = await _productService.UpdateAsync(product);
                //    return Ok(productResult);

                default:
                    return BadRequest("Invalid object type.");
            }
        }

        private async Task<IActionResult> HandleDelete(string objectType, long id)
        {
            switch (objectType)
            {
                case "customer":
                    var customerResult = await _customerService.DeleteAsync(id);
                    return Ok(customerResult);

                //case "order":
                //    var orderResult = await _orderService.DeleteAsync(id);
                //    return Ok(orderResult);

                //case "product":
                //    var productResult = await _productService.DeleteAsync(id);
                //    return Ok(productResult);

                default:
                    return BadRequest("Invalid object type.");
            }
        }
    }
}
