using IODynamicObject.Application.Enumeration;
using IODynamicObject.Application.Filters;
using IODynamicObject.Core.Metadata.Models;

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
