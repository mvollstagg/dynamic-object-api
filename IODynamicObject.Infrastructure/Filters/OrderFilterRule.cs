using IODynamicObject.Application.Filters;
using IODynamicObject.Application.Types.Orders;

namespace IODynamicObject.Infrastructure.Filters
{
    public class OrderFilterRule : IIOFilterRule
    {
        public IQueryable<object> ApplyFilters(IQueryable<object> source, object filter)
        {
            var orderFilter = filter as OrderFilter;
            if (orderFilter == null)
            {
                return source;
            }

            var query = source.Cast<Order>().AsQueryable();

            if (orderFilter.Id > 0)
            {
                query = query.Where(o => o.Id == orderFilter.Id);
            }
            if (orderFilter.CustomerId > 0)
            {
                query = query.Where(o => o.CustomerId == orderFilter.CustomerId);
            }
            if (orderFilter.OrderStatus > 0)
            {
                query = query.Where(o => o.OrderStatus == orderFilter.OrderStatus);
            }
            if (orderFilter.TotalAmount > 0)
            {
                query = query.Where(o => o.TotalAmount == orderFilter.TotalAmount);
            }

            return query.Cast<object>();
        }
    }
}