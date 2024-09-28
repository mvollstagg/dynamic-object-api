using System.Linq.Expressions;
using System.Reflection;

namespace IODynamicObject.Core.Sorting
{
    public class IOSortingHelper
    {
        public static IQueryable<TEntity> ApplySorting<TEntity>(IQueryable<TEntity> query, string sortBy, string sortOrder) where TEntity : class
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
    }
}
