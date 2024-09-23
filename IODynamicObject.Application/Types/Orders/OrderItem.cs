using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Application.Types.Orders
{
    public class OrderItem : BaseType
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
