using IODynamicObject.Application.Types.Customers;
using IODynamicObject.Core.Filtering;
using IODynamicObject.Domain.Entities;

namespace IODynamicObject.Infrastructure.Filters
{
    public class CustomerFilterRule : IIOFilterRule<IOCustomer, CustomerFilter>
    {
        public IQueryable<IOCustomer> ApplyFilters(IQueryable<IOCustomer> source, CustomerFilter filter)
        {
            if (filter == null)
            {
                return source;
            }

            if (filter.Id > 0)
            {
                source = source.Where(c => c.Id == filter.Id);
            }
            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                source = source.Where(c => c.FirstName.Contains(filter.FirstName));
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                source = source.Where(c => c.LastName.Contains(filter.LastName));
            }
            if (!string.IsNullOrEmpty(filter.Email))
            {
                source = source.Where(c => c.Email.Contains(filter.Email));
            }
            if (!string.IsNullOrEmpty(filter.Phone))
            {
                source = source.Where(c => c.Phone.Contains(filter.Phone));
            }
            if (!string.IsNullOrEmpty(filter.Address))
            {
                source = source.Where(c => c.Address.Contains(filter.Address));
            }

            return source;
        }
    }
}
