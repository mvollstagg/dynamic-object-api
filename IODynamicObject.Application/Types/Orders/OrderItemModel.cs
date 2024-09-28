using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Application.Types.Orders
{
    public class OrderItemModel : IOEntityBase
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
