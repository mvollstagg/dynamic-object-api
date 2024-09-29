using IODynamicObject.Application.Types.Products;
using IODynamicObject.Core.Attributes;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Domain.Entities;

namespace IODynamicObject.Application.Types.Orders
{
    public class OrderItemModel : IOEntityBase
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        [IOMappingPropertyType(typeof(IOProduct))]
        public ProductModel Product { get; set; }
    }
}
