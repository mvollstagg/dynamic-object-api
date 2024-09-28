using IODynamicObject.Application.Types.Products;
using IODynamicObject.Core.Filtering;
using IODynamicObject.Domain.Entities;

namespace IODynamicObject.Infrastructure.Filters
{
    public class ProductFilterRule : IIOFilterRule<IOProduct, ProductFilter>
    {
        public IQueryable<IOProduct> ApplyFilters(IQueryable<IOProduct> source, ProductFilter filter)
        {
            if (filter == null)
            {
                return source;
            }

            if (filter.Id > 0)
            {
                source = source.Where(c => c.Id == filter.Id);
            }
            if (!string.IsNullOrEmpty(filter.Name))
            {
                source = source.Where(p => p.Name.Contains(filter.Name));
            }
            if (!string.IsNullOrEmpty(filter.Brand))
            {
                source = source.Where(p => p.Brand.Contains(filter.Brand));
            }
            if (!string.IsNullOrEmpty(filter.Category))
            {
                source = source.Where(p => p.Category.Contains(filter.Category));
            }
            if (filter.Price > 0)
            {
                source = source.Where(p => p.Price == filter.Price);
            }

            return source;
        }
    }
}
