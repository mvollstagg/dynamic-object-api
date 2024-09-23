using IODynamicObject.Application.Filters;
using IODynamicObject.Application.Types.Orders;

namespace IODynamicObject.Infrastructure.Filters
{
    public class OrderFilterRule : IIOFilterRule<Order, OrderFilter>
    {
        public IQueryable<Order> ApplyFilters(IQueryable<object> source, object filter)
        {
            var orderFilter = filter as OrderFilter;
            var query = source.Cast<Order>().AsQueryable();

            if (filter == null)
            {
                return query;
            }

            if (orderFilter.Guid != Guid.Empty)
            {
                query = query.Where(c => c.Guid == orderFilter.Guid);
            }
            if (orderFilter.CustomerId != Guid.Empty)
            {
                query = query.Where(o => o.CustomerId == orderFilter.CustomerId);
            }
            if (orderFilter.OrderDateUtc.HasValue)
            {
                query = query.Where(o => o.OrderDateUtc.Date == orderFilter.OrderDateUtc.Value.Date);
            }
            if (orderFilter.OrderStatus.HasValue)
            {
                query = query.Where(o => o.OrderStatus == orderFilter.OrderStatus.Value);
            }
            if (orderFilter.TotalAmount > 0)
            {
                query = query.Where(o => o.TotalAmount == orderFilter.TotalAmount);
            }

            return query;
        }
    }
}
