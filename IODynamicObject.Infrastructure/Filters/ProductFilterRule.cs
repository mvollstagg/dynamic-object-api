using IODynamicObject.Application.Filters;
using IODynamicObject.Application.Types.Products;

namespace IODynamicObject.Infrastructure.Filters
{
    public class ProductFilterRule : IIOFilterRule
    {
        public IQueryable<object> ApplyFilters(IQueryable<object> source, object filter)
        {
            var productFilter = filter as ProductFilter;
            if (productFilter == null)
            {
                return source;
            }

            var query = source.Cast<Product>().AsQueryable();

            if (productFilter.Id > 0)
            {
                query = query.Where(p => p.Id == productFilter.Id);
            }
            if (!string.IsNullOrEmpty(productFilter.Name))
            {
                query = query.Where(p => p.Name == productFilter.Name);
            }
            if (!string.IsNullOrEmpty(productFilter.Brand))
            {
                query = query.Where(p => p.Brand == productFilter.Brand);
            }
            if (productFilter.Price > 0)
            {
                query = query.Where(p => p.Price == productFilter.Price);
            }
            if (!string.IsNullOrEmpty(productFilter.Category))
            {
                query = query.Where(p => p.Category == productFilter.Category);
            }

            return query.Cast<object>();
        }
    }
}
