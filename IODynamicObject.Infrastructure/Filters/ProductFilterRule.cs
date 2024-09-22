using IODynamicObject.Application.Filters;
using IODynamicObject.Application.Types.Products;d
using IODynamicObject.Core.Filtering;

namespace IODynamicObject.Infrastructure.Filters
{
    public class ProductFilterRule : IIOFilterRule<Product, ProductFilter>
    {
        public IQueryable<Product> ApplyFilters(IQueryable<Product> source, ProductFilter filter)
        {
            if (filter == null)
            {
                return source;
            }

            if (filter.Id > 0)
            {
                source = source.Where(x => x.Id == filter.Id);
            }
            if (filter.Price > 0)
            {
                source = source.Where(x => x.Price == filter.Price);
            }
            if (!string.IsNullOrEmpty(filter.Name))
            {
                source = source.Where(x => x.Name == filter.Name);
            }
            if (!string.IsNullOrEmpty(filter.Brand))
            {
                source = source.Where(x => x.Brand == filter.Brand);
            }
            if (!string.IsNullOrEmpty(filter.Category))
            {
                source = source.Where(x => x.Category == filter.Category);
            }

            return source;
        }
    }
}
