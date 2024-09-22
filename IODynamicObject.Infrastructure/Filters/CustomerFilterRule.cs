using IODynamicObject.Application.Filters;
using IODynamicObject.Application.Types.Customers;
using IODynamicObject.Core.Filtering;

namespace IODynamicObject.Infrastructure.Filters
{
    public class CustomerFilterRule : IIOFilterRule<Customer, CustomerFilter>
    {
        public IQueryable<Customer> ApplyFilters(IQueryable<Customer> source, CustomerFilter filter)
        {
            if (filter == null)
            {
                return source;
            }

            if (filter.Id > 0)
            {
                source = source.Where(x => x.Id == filter.Id);
            }
            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                source = source.Where(x => x.FirstName == filter.FirstName);
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                source = source.Where(x => x.LastName == filter.LastName);
            }
            if (!string.IsNullOrEmpty(filter.Email))
            {
                source = source.Where(x => x.Email == filter.Email);
            }
            if (!string.IsNullOrEmpty(filter.Phone))
            {
                source = source.Where(x => x.Phone == filter.Phone);
            }
            if (!string.IsNullOrEmpty(filter.Address))
            {
                source = source.Where(x => x.Address == filter.Address);
            }

            return source;
        }
    }
}
