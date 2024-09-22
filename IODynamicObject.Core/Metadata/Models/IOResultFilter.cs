using System.Linq.Expressions;
using System.Reflection;

namespace IODynamicObject.Core.Metadata.Models
{
    public class IOResultFilter<T>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Id";
        public string SortOrder { get; set; } = "asc";

        public List<T> Data { get; set; } = new List<T>();

        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }

        public static IOResultFilter<T> Create(IQueryable<T> source, int pageNumber, int pageSize, string sortBy, string sortOrder)
        {
            var result = new IOResultFilter<T>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            // Apply sorting
            source = ApplySorting(source, sortBy, sortOrder);

            // Get total count
            int totalCount = source.Count();

            // Apply pagination
            result.Data = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Set HasNext and HasPrevious
            result.HasNext = (pageNumber * pageSize) < totalCount;
            result.HasPrevious = pageNumber > 1;

            return result;
        }

        private static IQueryable<T> ApplySorting(IQueryable<T> source, string sortBy, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return source;
            }

            // Use System.Linq.Dynamic.Core for dynamic sorting
            string orderExpression = $"{sortBy} {sortOrder}";
            return source.OrderBy(orderExpression);
        }
    }

}
