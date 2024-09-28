using System.ComponentModel;

namespace IODynamicObject.Domain.Enumeration
{
    public enum OperationTypeEnum : byte
    {
        [Description("Create")]
        Create = 1,
        [Description("Read")]
        Read = 2,
        [Description("Update")]
        Update = 3,
        [Description("Delete")]
        Delete = 4
    }
}
