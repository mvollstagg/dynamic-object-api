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
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Reflection;
using IODynamicObject.Infrastructure.Filters;

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

        public async Task<IOResult<IODynamicObjectEntity>> GetByGuidAsync(SchemaTypeEnum schemaType, Guid guid)
        {
            try
            {
                // Find all objects with the given schema type
                var dynamicObjects = await _context.IODynamicObjects
                    .Where(o => o.SchemaType == schemaType && !o.Deleted)
                    .ToListAsync();

                // Loop through each object, deserialize and check for matching Guid
                foreach (var dynamicObject in dynamicObjects)
                {
                    switch (schemaType)
                    {
                        case SchemaTypeEnum.Customer:
                            var customer = JsonConvert.DeserializeObject<Customer>(dynamicObject.Data);
                            if (customer.Guid == guid)
                            {
                                return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Success, dynamicObject);
                            }
                            break;
                        case SchemaTypeEnum.Product:
                            var product = JsonConvert.DeserializeObject<Product>(dynamicObject.Data);
                            if (product.Guid == guid)
                            {
                                return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Success, dynamicObject);
                            }
                            break;
                        case SchemaTypeEnum.Order:
                            var order = JsonConvert.DeserializeObject<Order>(dynamicObject.Data);
                            if (order.Guid == guid)
                            {
                                return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Success, dynamicObject);
                            }
                            break;
                    }
                }

                return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Error, "Object with the specified Guid not found.");
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
                var sortBy = (filter as BaseFilter).SortBy;
                var sortOrder = (filter as BaseFilter).SortOrder;
                var pageNumber = (filter as BaseFilter).PageNumber;
                var pageSize = (filter as BaseFilter).PageSize;
                // Deserialize Data field into the appropriate type
                switch (schemaType)
                {
                    case SchemaTypeEnum.Customer:
                        deserializedObjects = dynamicObjects
                            .Select(o => JsonConvert.DeserializeObject<Customer>(o.Data));

                        // Apply filters
                        deserializedObjects = new CustomerFilterRule().ApplyFilters(deserializedObjects.AsQueryable(), filter as CustomerFilter);

                        // Cast to concrete type before applying sorting
                        deserializedObjects = ApplySorting((deserializedObjects as IEnumerable<Customer>).AsQueryable(), sortBy, sortOrder);
                        break;

                    case SchemaTypeEnum.Order:
                        deserializedObjects = dynamicObjects
                            .Select(o => JsonConvert.DeserializeObject<Order>(o.Data));

                        // Apply filters
                        deserializedObjects = new OrderFilterRule().ApplyFilters(deserializedObjects.AsQueryable(), filter as OrderFilter);

                        // Cast to concrete type before applying sorting
                        deserializedObjects = ApplySorting((deserializedObjects as IEnumerable<Order>).AsQueryable(), sortBy, sortOrder);
                        break;

                    case SchemaTypeEnum.Product:
                        deserializedObjects = dynamicObjects
                            .Select(o => JsonConvert.DeserializeObject<Product>(o.Data));

                        // Apply filters
                        deserializedObjects = new ProductFilterRule().ApplyFilters(deserializedObjects.AsQueryable(), filter as ProductFilter);

                        // Cast to concrete type before applying sorting
                        deserializedObjects = ApplySorting((deserializedObjects as IEnumerable<Product>).AsQueryable(), sortBy, sortOrder);
                        break;

                    default:
                        throw new Exception("Unsupported schema type.");
                }

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

        // Helper method for sorting
        private IQueryable<TEntity> ApplySorting<TEntity>(IQueryable<TEntity> query, string sortBy, string sortOrder) where TEntity : class
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return query;
            }

            // Ensure that we're working with the actual entity type, not dynamic
            var entityType = typeof(TEntity);

            // Get the property to sort by
            var propertyInfo = entityType.GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            // If property doesn't exist, throw an exception
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property '{sortBy}' does not exist on type '{entityType.Name}'.", nameof(sortBy));
            }

            // Create the lambda expression for sorting
            var parameter = Expression.Parameter(entityType, "x");
            var property = Expression.Property(parameter, propertyInfo);
            var lambda = Expression.Lambda(property, parameter);

            // Determine the sorting method (OrderBy or OrderByDescending)
            string methodName = sortOrder.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

            // Get the generic method for sorting
            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            // Invoke the sorting method
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
