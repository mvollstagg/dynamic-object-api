using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Domain.Enumeration;

namespace IODynamicObject.Application.Types.IODynamicObjects
{
    public class IODynamicObjectRequest
    {
        public OperationTypeEnum Operation { get; set; }
        public SchemaTypeEnum Schema { get; set; }
        public dynamic Data { get; set; }
    }
}
