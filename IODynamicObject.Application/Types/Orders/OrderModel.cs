using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Domain.Enumeration;

namespace IODynamicObject.Application.Types.Orders
{
    public class OrderModel : IOEntityBase
    {
        public long CustomerId { get; set; }
        public DateTime OrderDateUtc { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemModel> OrderItems { get; set; }
    }
}
