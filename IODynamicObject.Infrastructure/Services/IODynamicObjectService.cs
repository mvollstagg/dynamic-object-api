using IODynamicObject.Application.Interfaces.Services;
using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using IODynamicObjectEntity = IODynamicObject.Domain.Entities.IODynamicObject;
using IODynamicObject.Application.Types.Customers;
using System.Linq.Expressions;
using IODynamicObject.Application.Types.Orders;
using IODynamicObject.Application.Types.Products;
using Newtonsoft.Json;
using IODynamicObject.Application.Filters;

namespace IODynamicObject.Infrastructure.Services
{
    public class IODynamicObjectService : IIODynamicObjectService
    {
        private readonly IODataContext _context;

        public IODynamicObjectService(IODataContext context)
        {
            _context = context;
        }

        public async Task<IOResult<IODynamicObjectEntity>> CreateAsync(IODynamicObjectEntity dynamicObject)
        {
            try
            {
                dynamicObject.CreationDateUtc = DateTime.UtcNow;
                dynamicObject.ModificationDateUtc = DateTime.UtcNow;

                await _context.IODynamicObjects.AddAsync(dynamicObject);
                await _context.SaveChangesAsync();

                return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Success, dynamicObject);
            }
            catch (Exception ex)
            {
                return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<object>> GetByFiltersAsync(
            SchemaTypeEnum schemaType,
            object filter)
        {
            try
            {
                // Retrieve all dynamic objects of the specified schema type
                var dynamicObjects = await _context.IODynamicObjects
                    .Where(o => o.SchemaType == schemaType && !o.Deleted)
                    .ToListAsync();

                IEnumerable<dynamic> deserializedObjects;

                // Deserialize Data field into the appropriate type
                switch (schemaType)
                {
                    case SchemaTypeEnum.Customer:
                        deserializedObjects = dynamicObjects
                            .Select(o => JsonConvert.DeserializeObject<Customer>(o.Data))
                            .AsQueryable();
                        deserializedObjects = ApplyCustomerFilters(deserializedObjects, filter as CustomerFilter);
                        break;

                    case SchemaTypeEnum.Order:
                        deserializedObjects = dynamicObjects
                            .Select(o => JsonConvert.DeserializeObject<Order>(o.Data))
                            .AsQueryable();
                        deserializedObjects = ApplyOrderFilters(deserializedObjects, filter as OrderFilter);
                        break;

                    case SchemaTypeEnum.Product:
                        deserializedObjects = dynamicObjects
                            .Select(o => JsonConvert.DeserializeObject<Product>(o.Data))
                            .AsQueryable();
                        deserializedObjects = ApplyProductFilters(deserializedObjects, filter as ProductFilter);
                        break;

                    default:
                        throw new Exception("Unsupported schema type.");
                }

                var sortBy = (filter as BaseFilter).SortBy;
                var sortOrder = (filter as BaseFilter).SortOrder;
                var pageNumber = (filter as BaseFilter).PageNumber;
                var pageSize = (filter as BaseFilter).PageSize;
                // Apply sorting
                deserializedObjects = ApplySorting(deserializedObjects.AsQueryable(), sortBy, sortOrder);

                // Get total count
                int totalCount = deserializedObjects.Count();

                // Apply pagination
                var pagedData = deserializedObjects
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Prepare the metadata
                var metaData = new IOResultMetadata
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    HasNext = (pageNumber * pageSize) < totalCount,
                    HasPrevious = pageNumber > 1,
                    Status = IOResultStatusEnum.Success
                };

                // Prepare the result
                var result = new IOResult<object>
                {
                    Meta = metaData,
                    Data = pagedData
                };

                return result;
            }
            catch (Exception ex)
            {
                var errorResult = new IOResult<object>
                {
                    Meta = new IOResultMetadata
                    {
                        Status = IOResultStatusEnum.Error,
                        Messages = new List<KeyValuePair<string, List<string>>>
                        {
                            new KeyValuePair<string, List<string>>("Exception", new List<string> { ex.Message })
                        }
                    },
                    Data = null
                };
                return errorResult;
            }
        }

        // Helper methods for filtering
        private IQueryable<Customer> ApplyCustomerFilters(IEnumerable<dynamic> source, CustomerFilter filter)
        {
            var query = source.Cast<Customer>().AsQueryable();

            if (filter == null)
            {
                return query;
            }

            if (filter.Id > 0)
            {
                query = query.Where(c => c.Id == filter.Id);
            }
            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                query = query.Where(c => c.FirstName.Contains(filter.FirstName));
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                query = query.Where(c => c.LastName.Contains(filter.LastName));
            }
            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(c => c.Email.Contains(filter.Email));
            }
            if (!string.IsNullOrEmpty(filter.Phone))
            {
                query = query.Where(c => c.Phone.Contains(filter.Phone));
            }
            if (!string.IsNullOrEmpty(filter.Address))
            {
                query = query.Where(c => c.Address.Contains(filter.Address));
            }

            return query;
        }

        private IQueryable<Order> ApplyOrderFilters(IEnumerable<dynamic> source, OrderFilter filter)
        {
            var query = source.Cast<Order>().AsQueryable();

            if (filter == null)
            {
                return query;
            }

            if (filter.Id > 0)
            {
                query = query.Where(o => o.Id == filter.Id);
            }
            if (filter.CustomerId > 0)
            {
                query = query.Where(o => o.CustomerId == filter.CustomerId);
            }
            if (filter.OrderDateUtc.HasValue)
            {
                query = query.Where(o => o.OrderDateUtc.Date == filter.OrderDateUtc.Value.Date);
            }
            if (filter.OrderStatus.HasValue)
            {
                query = query.Where(o => o.OrderStatus == filter.OrderStatus.Value);
            }
            if (filter.TotalAmount > 0)
            {
                query = query.Where(o => o.TotalAmount == filter.TotalAmount);
            }

            return query;
        }

        private IQueryable<Product> ApplyProductFilters(IEnumerable<dynamic> source, ProductFilter filter)
        {
            var query = source.Cast<Product>().AsQueryable();

            if (filter == null)
            {
                return query;
            }

            if (filter.Id > 0)
            {
                query = query.Where(p => p.Id == filter.Id);
            }
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(p => p.Name.Contains(filter.Name));
            }
            if (!string.IsNullOrEmpty(filter.Brand))
            {
                query = query.Where(p => p.Brand.Contains(filter.Brand));
            }
            if (!string.IsNullOrEmpty(filter.Category))
            {
                query = query.Where(p => p.Category.Contains(filter.Category));
            }
            if (filter.Price > 0)
            {
                query = query.Where(p => p.Price == filter.Price);
            }

            return query;
        }

        // Helper method for sorting
        private IQueryable<TEntity> ApplySorting<TEntity>(IQueryable<TEntity> query, string sortBy, string sortOrder) where TEntity : class
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return query;
            }

            var entityType = typeof(TEntity);
            var propertyInfo = entityType.GetProperty(sortBy);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{sortBy}' does not exist on type '{entityType.Name}'.", nameof(sortBy));
            }

            var parameter = Expression.Parameter(entityType, "x");
            var property = Expression.Property(parameter, propertyInfo);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = sortOrder.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(entityType, property.Type);

            return (IQueryable<TEntity>)method.Invoke(null, new object[] { query, lambda });
        }

        public async Task<IOResult<IODynamicObjectEntity>> UpdateAsync(IODynamicObjectEntity dynamicObject)
        {
            try
            {
                var existingObject = await _context.IODynamicObjects.FindAsync(dynamicObject.Id);

                if (existingObject == null || existingObject.Deleted)
                {
                    return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Error, "Object not found.");
                }

                existingObject.Data = dynamicObject.Data;
                existingObject.SchemaType = dynamicObject.SchemaType;
                existingObject.ModificationDateUtc = DateTime.UtcNow;

                _context.IODynamicObjects.Update(existingObject);
                await _context.SaveChangesAsync();

                return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Success, existingObject);
            }
            catch (Exception ex)
            {
                return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<bool>> DeleteAsync(long id)
        {
            var dynamicObject = await _context.IODynamicObjects.FindAsync(id);

            if (dynamicObject == null || dynamicObject.Deleted)
            {
                return new IOResult<bool>(IOResultStatusEnum.Error, false, "Object not found.");
            }

            dynamicObject.Deleted = true;
            dynamicObject.ModificationDateUtc = DateTime.UtcNow;

            _context.IODynamicObjects.Update(dynamicObject);
            await _context.SaveChangesAsync();

            return new IOResult<bool>(IOResultStatusEnum.Success, true);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
