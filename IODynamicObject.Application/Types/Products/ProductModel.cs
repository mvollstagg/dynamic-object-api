using IODynamicObject.Application.Types.Orders;
using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Application.Types.Products
{
    public class ProductModel : IOEntityBase
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }

        public List<Dictionary<string, List<Dictionary<string, string>>>> DynamicObjects { get; set; }
        public DateTime CreationDateUtc { get; set; }
        public DateTime ModificationDateUtc { get; set; }
    }
}
