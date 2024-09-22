using IODynamicObject.Core.Metadata.Models;

namespace IODynamicObject.Application.Filters
{
    public interface IOFilter<T> where T : IOEntityBase, new()
    {
    }
}
