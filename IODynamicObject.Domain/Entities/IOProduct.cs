using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Domain.Entities
{
    public class IOProduct : IOEntityDeletable
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        // Navigation property for dynamic objects (custom fields for a product)
        public ICollection<IOObject> DynamicObjects { get; set; }
    }
}
