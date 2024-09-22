using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Domain.Entities
{
    public class IODynamicObject : IOEntityDeletable
    {
        public SchemaTypeEnum SchemaType { get; set; }
        public string Data { get; set; } // JSON string
    }
}
