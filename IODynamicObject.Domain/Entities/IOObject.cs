using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Domain.Entities
{
    public class IOObject : IOEntityTrackable
    {
        public string Name { get; set; }

        public virtual List<IOField> Fields { get; set; }
    }
}
