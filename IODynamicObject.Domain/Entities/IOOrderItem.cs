using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Domain.Entities
{
    public class IOOrderItem : IOEntityBase
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public IOOrder Order { get; set; }
        public IOProduct Product { get; set; }
        public virtual List<IOObject> DynamicObjects { get; set; }
    }
}
