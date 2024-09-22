namespace IODynamicObject.Application.Filters
{
    public interface IIOFilterRule<TEntity, TFilter>
    {
        IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> source, TFilter filters);
    }
}
