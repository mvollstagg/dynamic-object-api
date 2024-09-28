using IODynamicObject.Core.Filtering;

namespace IODynamicObject.Application.Types.Products
{
    public class ProductFilter : BaseFilter
    {
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public decimal? Price { get; set; }
        public string?   Category { get; set; }
    }
}
