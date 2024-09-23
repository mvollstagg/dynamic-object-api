using IODynamicObject.Application.Filters;
using IODynamicObject.Application.Types.Customers;

namespace IODynamicObject.Infrastructure.Filters
{
    public class CustomerFilterRule : IIOFilterRule<Customer, CustomerFilter>
    {
        public IQueryable<Customer> ApplyFilters(IQueryable<object> source, object filter)
        {
            var customerFilter = filter as CustomerFilter;
            var query = source.Cast<Customer>().AsQueryable();

            if (filter == null)
            {
                return query;
            }

            if (customerFilter.Guid != Guid.Empty)
            {
                query = query.Where(c => c.Guid == customerFilter.Guid);
            }
            if (!string.IsNullOrEmpty(customerFilter.FirstName))
            {
                query = query.Where(c => c.FirstName.Contains(customerFilter.FirstName));
            }
            if (!string.IsNullOrEmpty(customerFilter.LastName))
            {
                query = query.Where(c => c.LastName.Contains(customerFilter.LastName));
            }
            if (!string.IsNullOrEmpty(customerFilter.Email))
            {
                query = query.Where(c => c.Email.Contains(customerFilter.Email));
            }
            if (!string.IsNullOrEmpty(customerFilter.Phone))
            {
                query = query.Where(c => c.Phone.Contains(customerFilter.Phone));
            }
            if (!string.IsNullOrEmpty(customerFilter.Address))
            {
                query = query.Where(c => c.Address.Contains(customerFilter.Address));
            }

            return query;
        }
    }
}
