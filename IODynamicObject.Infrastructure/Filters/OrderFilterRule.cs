using IODynamicObject.Application.Filters;
using IODynamicObject.Application.Types.Orders;
using IODynamicObject.Core.Filtering;

namespace IODynamicObject.Infrastructure.Filters
{
    public class OrderFilterRule : IIOFilterRule<Order, OrderFilter>
    {
        public IQueryable<Order> ApplyFilters(IQueryable<Order> source, OrderFilter filter)
        {
            if (filter == null)
            {
                return source;
            }

            if (filter.Id > 0)
            {
                source = source.Where(x => x.Id == filter.Id);
            }
            if (filter.CustomerId > 0)
            {
                source = source.Where(x => x.CustomerId == filter.CustomerId);
            }
            if (filter.OrderStatus > 0)
            {
                source = source.Where(x => x.OrderStatus == filter.OrderStatus);
            }

            return source;
        }
    }
}