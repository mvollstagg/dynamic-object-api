using IODynamicObject.Application.Filters;
using IODynamicObject.Application.Types.Customers;

namespace IODynamicObject.Infrastructure.Filters
{
    public class CustomerFilterRule : IIOFilterRule
    {
        public IQueryable<object> ApplyFilters(IQueryable<object> source, object filter)
        {
            var customerFilter = filter as CustomerFilter;
            if (customerFilter == null)
            {
                return source;
            }

            var query = source.Cast<Customer>().AsQueryable();

            if (customerFilter.Id > 0)
            {
                query = query.Where(c => c.Id == customerFilter.Id);
            }
            if (!string.IsNullOrEmpty(customerFilter.FirstName))
            {
                query = query.Where(c => c.FirstName == customerFilter.FirstName);
            }
            if (!string.IsNullOrEmpty(customerFilter.LastName))
            {
                query = query.Where(c => c.LastName == customerFilter.LastName);
            }
            if (!string.IsNullOrEmpty(customerFilter.Email))
            {
                query = query.Where(c => c.Email == customerFilter.Email);
            }
            if (!string.IsNullOrEmpty(customerFilter.Phone))
            {
                query = query.Where(c => c.Phone == customerFilter.Phone);
            }
            if (!string.IsNullOrEmpty(customerFilter.Address))
            {
                query = query.Where(c => c.Address == customerFilter.Address);
            }

            return query.Cast<object>();
        }
    }
}
