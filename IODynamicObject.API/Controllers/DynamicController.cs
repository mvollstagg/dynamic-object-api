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
using IODynamicObject.Application.Mappers;

namespace IODynamicObject.API.Controllers
{
    [ApiController]
    [Route("api/dynamic")]
    public class DynamicController : ControllerBase
    {
        private readonly IOCustomerService _customerService;
        private readonly IOProductService _productService;
        private readonly IOOrderService _orderService;

        public DynamicController(IOCustomerService customerService,
                                IOProductService productService,
                                IOOrderService orderService)
        {
            _customerService = customerService;
            _productService = productService;
            _orderService = orderService;
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
                    var customerResult = await _customerService.CreateAsync(customer);
                    var customerDto = await HandleRead("customer", customerResult.Data.Id);
                    return Ok(customerDto);

                case "order":
                    var order = data.ToObject<IOOrder>();
                    var orderResult = await _orderService.CreateAsync(order);
                    var orderDto = await HandleRead("order", orderResult.Data.Id);
                    return Ok(orderDto);

                case "product":
                    var product = data.ToObject<IOProduct>();
                    var productResult = await _productService.CreateAsync(product);
                    var productDto = await HandleRead("product", productResult.Data.Id);
                    return Ok(productDto);

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
                    var customerDto = IOMappingHelper.ApplyMapping<IOCustomer, CustomerModel>(customerResult.Data, "Customer");
                    return Ok(customerDto);

                case "order":
                    var orderResult = await _orderService.GetByIdAsync(id);
                    var orderDto = IOMappingHelper.ApplyMapping<IOOrder, OrderModel>(orderResult.Data);
                    return Ok(orderDto);

                case "product":
                    var productResult = await _productService.GetByIdAsync(id);
                    var productDto = IOMappingHelper.ApplyMapping<IOProduct, ProductModel>(productResult.Data);
                    return Ok(productDto);

                default:
                    return BadRequest("Invalid object type.");
            }
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

                case "product":
                    var product = data.ToObject<IOProduct>();
                    product.Id = id;
                    var productResult = await _productService.UpdateAsync(product);
                    return Ok(productResult);

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

                case "product":
                    var productResult = await _productService.DeleteAsync(id);
                    return Ok(productResult);

                default:
                    return BadRequest("Invalid object type.");
            }
        }
    }
}
