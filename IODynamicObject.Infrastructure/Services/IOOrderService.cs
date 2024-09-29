using IODynamicObject.Application.Interfaces.Services;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Infrastructure.Persistence;
using IODynamicObject.Application.Types.Orders;
using IODynamicObject.Infrastructure.Filters;
using IODynamicObject.Core.Filtering;

namespace IODynamicObject.Infrastructure.Services
{
    public class IOOrderService : IIODynamicObjectService<IOOrder, OrderFilter>
    {
        private readonly IODataContext _context;

        public IOOrderService(IODataContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<IOResult<IOOrder>> CreateAsync(IOOrder order)
        {
            using var transaction = await BeginTransactionAsync();

            try
            {
                order.CreationDateUtc = DateTime.UtcNow;
                order.ModificationDateUtc = DateTime.UtcNow;

                // Process dynamic objects (e.g., custom order fields)
                if (order.DynamicObjects != null && order.DynamicObjects.Any())
                {
                    foreach (var dynamicObject in order.DynamicObjects)
                    {
                        dynamicObject.CreationDateUtc = DateTime.UtcNow;
                        dynamicObject.ModificationDateUtc = DateTime.UtcNow;

                        foreach (var field in dynamicObject.Fields)
                        {
                            field.CreationDateUtc = DateTime.UtcNow;
                            field.ModificationDateUtc = DateTime.UtcNow;

                            foreach (var value in field.Values)
                            {
                                value.CreationDateUtc = DateTime.UtcNow;
                                value.ModificationDateUtc = DateTime.UtcNow;
                            }
                        }
                    }
                }

                // Add order with its order items and dynamic objects
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new IOResult<IOOrder>(IOResultStatusEnum.Success, order);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new IOResult<IOOrder>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<IOOrder>> GetByIdAsync(long id)
        {
            try
            {
                // Find order by ID and include related customer, items, and dynamic objects
                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Include(o => o.DynamicObjects)
                    .ThenInclude(o => o.Fields)
                    .ThenInclude(f => f.Values)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null || order.Deleted)
                {
                    return new IOResult<IOOrder>(IOResultStatusEnum.Error, "Order not found.");
                }

                return new IOResult<IOOrder>(IOResultStatusEnum.Success, order);
            }
            catch (Exception ex)
            {
                return new IOResult<IOOrder>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<List<IOOrder>>> GetByFiltersAsync(OrderFilter filter)
        {
            try
            {
                // Retrieve all orders that aren't deleted
                var orders = _context.Orders.Where(o => !o.Deleted);

                // Apply filters here (you would extend this with your own filter logic)
                if (filter != null)
                {
                    orders = IOFilteringHelper<IOOrder, OrderFilter, OrderFilterRule>.ApplyFilter(orders, filter);
                }

                var resultList = await orders.ToListAsync();

                return new IOResult<List<IOOrder>>(IOResultStatusEnum.Success, resultList);
            }
            catch (Exception ex)
            {
                return new IOResult<List<IOOrder>>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<IOOrder>> UpdateAsync(IOOrder order)
        {
            using var transaction = await BeginTransactionAsync();

            try
            {
                var existingOrder = await _context.Orders.FindAsync(order.Id);

                if (existingOrder == null || existingOrder.Deleted)
                {
                    return new IOResult<IOOrder>(IOResultStatusEnum.Error, "Order not found.");
                }

                // Update basic order details
                existingOrder.OrderDateUtc = order.OrderDateUtc;
                existingOrder.OrderStatus = order.OrderStatus;
                existingOrder.TotalAmount = order.TotalAmount;
                existingOrder.ModificationDateUtc = DateTime.UtcNow;

                // Update dynamic objects if needed
                if (order.DynamicObjects != null && order.DynamicObjects.Any())
                {
                    foreach (var dynamicObject in order.DynamicObjects)
                    {
                        dynamicObject.ModificationDateUtc = DateTime.UtcNow;

                        foreach (var field in dynamicObject.Fields)
                        {
                            field.ModificationDateUtc = DateTime.UtcNow;

                            foreach (var value in field.Values)
                            {
                                value.ModificationDateUtc = DateTime.UtcNow;
                            }
                        }
                    }
                }

                _context.Orders.Update(existingOrder);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new IOResult<IOOrder>(IOResultStatusEnum.Success, existingOrder);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new IOResult<IOOrder>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<bool>> DeleteAsync(long id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);

                if (order == null || order.Deleted)
                {
                    return new IOResult<bool>(IOResultStatusEnum.Error, false, "Order not found.");
                }

                // Mark as deleted
                order.Deleted = true;
                order.ModificationDateUtc = DateTime.UtcNow;

                _context.Orders.Update(order);
                await _context.SaveChangesAsync();

                return new IOResult<bool>(IOResultStatusEnum.Success, true);
            }
            catch (Exception ex)
            {
                return new IOResult<bool>(IOResultStatusEnum.Error, false, ex.Message);
            }
        }
    }
}
