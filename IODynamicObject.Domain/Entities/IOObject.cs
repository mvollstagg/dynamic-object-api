using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Domain.Entities
{
    public class IOObject : IOEntityBase
    {
        public string Name { get; set; } // E.g., Product, Customer, Order

        // Navigation property for related fields and values
        public ICollection<IOField> Fields { get; set; }
    }
}
