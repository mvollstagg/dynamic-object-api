using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Domain.Entities
{
    public class IOOrderItem : IOEntityBase
    {
        public long OrderId { get; set; } // Foreign Key to Order
        public long ProductId { get; set; } // Foreign Key to Product
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Navigation property for the order
        public IOOrder Order { get; set; }

        // Navigation property for the product
        public IOProduct Product { get; set; }
    }
}
