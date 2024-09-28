using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Core.Filtering
{
    public class BaseFilter : IOEntityBase
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Id";
        public string SortOrder { get; set; } = "asc";
    }
}
