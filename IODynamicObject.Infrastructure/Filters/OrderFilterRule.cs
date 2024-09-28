using IODynamicObject.Application.Types.Orders;
using IODynamicObject.Core.Filtering;
using IODynamicObject.Domain.Entities;

namespace IODynamicObject.Infrastructure.Filters
{
    public class OrderFilterRule : IIOFilterRule<IOOrder, OrderFilter>
    {
        public IQueryable<IOOrder> ApplyFilters(IQueryable<IOOrder> source, OrderFilter filter)
        {
            if (filter == null)
            {
                return source;
            }

            if (filter.Id > 0)
            {
                source = source.Where(c => c.Id == filter.Id);
            }
            if (filter.CustomerId > 0)
            {
                source = source.Where(o => o.CustomerId == filter.CustomerId);
            }
            if (filter.OrderDateUtc.HasValue)
            {
                source = source.Where(o => o.OrderDateUtc.Date == filter.OrderDateUtc.Value.Date);
            }
            if (filter.OrderStatus.HasValue)
            {
                source = source.Where(o => o.OrderStatus == filter.OrderStatus.Value);
            }
            if (filter.TotalAmount > 0)
            {
                source = source.Where(o => o.TotalAmount == filter.TotalAmount);
            }

            return source;
        }
    }
}
