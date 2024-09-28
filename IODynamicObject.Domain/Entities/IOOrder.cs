using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Domain.Enumeration;

namespace IODynamicObject.Domain.Entities
{
    public class IOOrder : IOEntityDeletable
    {
        public DateTime OrderDate { get; set; }
        public long CustomerId { get; set; } // Foreign Key to Customer
        public DateTime OrderDateUtc { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }

        // Navigation property for Customer
        public IOCustomer Customer { get; set; }

        // Navigation property for Order Items (sub-table)
        public ICollection<IOOrderItem> OrderItems { get; set; }

        // Navigation property for dynamic objects (custom fields for an order)
        public ICollection<IOObject> DynamicObjects { get; set; }
    }
}
