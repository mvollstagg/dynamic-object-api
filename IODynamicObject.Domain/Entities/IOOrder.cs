using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Domain.Enumeration;

namespace IODynamicObject.Domain.Entities
{
    public class IOOrder : IOEntityDeletable
    {
        public DateTime OrderDate { get; set; }
        public long CustomerId { get; set; }
        public DateTime OrderDateUtc { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }

        public IOCustomer Customer { get; set; }
        public virtual List<IOOrderItem> OrderItems { get; set; }
        public virtual List<IOObject> DynamicObjects { get; set; }
    }
}
