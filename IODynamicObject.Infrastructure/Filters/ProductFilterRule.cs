using IODynamicObject.Application.Filters;
using IODynamicObject.Application.Types.Products;

namespace IODynamicObject.Infrastructure.Filters
{
    public class ProductFilterRule : IIOFilterRule<Product, ProductFilter>
    {
        public IQueryable<Product> ApplyFilters(IQueryable<object> source, object filter)
        {
            var productFilter = filter as ProductFilter;
            var query = source.Cast<Product>().AsQueryable();

            if (filter == null)
            {
                return query;
            }

            if (productFilter.Guid != Guid.Empty)
            {
                query = query.Where(c => c.Guid == productFilter.Guid);
            }
            if (!string.IsNullOrEmpty(productFilter.Name))
            {
                query = query.Where(p => p.Name.Contains(productFilter.Name));
            }
            if (!string.IsNullOrEmpty(productFilter.Brand))
            {
                query = query.Where(p => p.Brand.Contains(productFilter.Brand));
            }
            if (!string.IsNullOrEmpty(productFilter.Category))
            {
                query = query.Where(p => p.Category.Contains(productFilter.Category));
            }
            if (productFilter.Price > 0)
            {
                query = query.Where(p => p.Price == productFilter.Price);
            }

            return query;
        }
    }
}
