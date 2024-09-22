namespace IODynamicObject.Application.DTOs.Responses
{
    public class IODynamicObjectFilter
    {
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public bool? HasNext { get; set; } = false;
        public bool? HasPrevious { get; set; } = false;
        public string? SortBy { get; set; } = "Id";
        public string? SortOrder { get; set; } = "asc";
    }
}
