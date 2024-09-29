
namespace IODynamicObject.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class IOMappingPropertyTypeAttribute : Attribute
    {
        public Type EntityType { get; }

        public IOMappingPropertyTypeAttribute(Type entityType)
        {
            EntityType = entityType;
        }
    }
}
