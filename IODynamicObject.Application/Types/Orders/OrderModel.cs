using IODynamicObject.Application.Types.Customers;
using IODynamicObject.Application.Types.Products;
using IODynamicObject.Core.Attributes;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Domain.Entities;
using IODynamicObject.Domain.Enumeration;

namespace IODynamicObject.Application.Types.Orders
{
    public class OrderModel : IOEntityBase
    {
        public long CustomerId { get; set; }
        public DateTime OrderDateUtc { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }

        [IOMappingPropertyType(typeof(IOCustomer))]
        public CustomerModel Customer { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
        public List<Dictionary<string, List<Dictionary<string, string>>>> DynamicObjects { get; set; }
        public DateTime CreationDateUtc { get; set; }
        public DateTime ModificationDateUtc { get; set; }
    }
}
