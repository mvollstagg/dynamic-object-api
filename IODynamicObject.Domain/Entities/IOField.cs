using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;
using Newtonsoft.Json.Linq;

namespace IODynamicObject.Domain.Entities
{
    public class IOField : IOEntityTrackable
    {
        public string Name { get; set; }

        public long ObjectId { get; set; }

        public virtual List<IOValue> Values { get; set; }
    }
}
