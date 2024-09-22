using System.ComponentModel;

namespace IODynamicObject.Application.Enumeration
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
