using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Core.Sorting;

namespace IODynamicObject.Core.Filtering
{
    public class IOFilteringHelper<T, TFilter, TFilterRule> where T : IOEntityBase 
                                                            where TFilter : BaseFilter 
                                                            where TFilterRule : IIOFilterRule<T, TFilter>, new()
    {
        public static IQueryable<T> ApplyFilter(IQueryable<T> query, TFilter filter)
        {
            var sortBy = filter.SortBy;
            var sortOrder = filter.SortOrder;
            var pageNumber = filter.PageNumber;
            var pageSize = filter.PageSize;

            // Apply filters
            query = new TFilterRule().ApplyFilters(query, filter);

            // Apply sorting
            query = IOSortingHelper.ApplySorting(query, sortBy, sortOrder);

            int totalCount = query.Count();
            // Apply pagination
            var pagedData = query
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

            return query;
        }
    }
}
