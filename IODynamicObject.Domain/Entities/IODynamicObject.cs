using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Domain.Entities
{
    public class IODynamicObject : IOEntityDeletable
    {
        public string ObjectType { get; set; }
        public string Data { get; set; } // JSON string
    }
}
