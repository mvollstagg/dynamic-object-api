namespace IODynamicObject.Application.Filters
{
    public interface IIOFilterRule
    {
        IQueryable<object> ApplyFilters(IQueryable<object> source, object filter);
    }
}
