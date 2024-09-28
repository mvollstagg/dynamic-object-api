using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;
using Newtonsoft.Json.Linq;

namespace IODynamicObject.Domain.Entities
{
    public class IOField : IOEntityBase
    {
        public string Name { get; set; } // E.g., Price, ScreenSize, Battery

        // Foreign key to IOObject
        public long ObjectId { get; set; }
        public IOObject Object { get; set; }

        // Navigation property for related values
        public ICollection<IOValue> Values { get; set; }
    }
}
