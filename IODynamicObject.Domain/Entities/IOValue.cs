using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Domain.Entities
{
    public class IOValue : IOEntityBase
    {
        public long FieldId { get; set; } // Foreign Key to IOField
        public IOField Field { get; set; }

        public string Value { get; set; } // Can store any value as a string
    }
}
