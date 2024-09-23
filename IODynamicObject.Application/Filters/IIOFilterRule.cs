namespace IODynamicObject.Application.Filters
{
    public interface IIOFilterRule<TObject, TFilter> where TObject : class where TFilter : class
    {
        IQueryable<TObject> ApplyFilters(IQueryable<object> source, object filter);
    }
}
