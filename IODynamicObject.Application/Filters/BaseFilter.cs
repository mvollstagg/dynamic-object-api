using IODynamicObject.Application.Types;
using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Application.Filters
{
    public class BaseFilter : BaseType
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Id";
        public string SortOrder { get; set; } = "asc";
    }
}
