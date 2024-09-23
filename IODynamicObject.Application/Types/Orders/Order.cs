using IODynamicObject.Application.Enumeration;
using IODynamicObject.Application.Filters;
using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Application.Types.Orders
{
    public class Order : BaseType
    {
        public Guid CustomerId { get; set; }
        public DateTime OrderDateUtc { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
