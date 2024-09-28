using System.ComponentModel;

namespace IODynamicObject.Domain.Enumeration
{
    public enum OrderStatusEnum : byte
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Processing")]
        Processing = 2,
        [Description("Shipped")]
        Shipped = 3,
        [Description("Delivered")]
        Delivered = 4,
        [Description("Cancelled")]
        Cancelled = 5
    }
}
