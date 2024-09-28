using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Core.Filtering
{
    public interface IIOFilterRule<TObject, TFilter> where TObject : IOEntityBase where TFilter : BaseFilter
    {
        IQueryable<TObject> ApplyFilters(IQueryable<TObject> source, TFilter filter);
    }
}
