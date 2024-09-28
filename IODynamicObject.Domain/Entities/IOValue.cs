using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Domain.Entities
{
    public class IOValue : IOEntityTrackable
    {
        public long FieldId { get; set; }
        public string Value { get; set; } 
    }
}
