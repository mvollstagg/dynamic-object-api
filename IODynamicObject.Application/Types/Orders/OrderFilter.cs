using IODynamicObject.Core.Filtering;
using IODynamicObject.Domain.Enumeration;

namespace IODynamicObject.Application.Types.Orders
{
    public class OrderFilter : BaseFilter
    {
        public long? CustomerId { get; set; }
        public OrderStatusEnum? OrderStatus { get; set; }
        public DateTime? OrderDateUtc { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
