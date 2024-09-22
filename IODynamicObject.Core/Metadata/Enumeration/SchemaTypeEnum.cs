using System.ComponentModel;

namespace IODynamicObject.Core.Metadata.Enumeration
{
    public enum SchemaTypeEnum : byte
    {
        [Description("Customer")]
        Customer = 1,
        [Description("Product")]
        Product = 2,
        [Description("Order")]
        Order = 3,
    }
}
