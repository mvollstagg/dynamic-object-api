using Microsoft.EntityFrameworkCore.Storage;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Core.Filtering;

namespace IODynamicObject.Application.Interfaces.Services
{
    public interface IIODynamicObjectService<T, TFilter> where T : IOEntityBase 
                                                        where TFilter : BaseFilter, 
                                                        new()
    {
        Task<IOResult<T>> CreateAsync(T dynamicObject);
        Task<IOResult<T>> GetByIdAsync(long id);
        Task<IOResult<List<T>>> GetByFiltersAsync(TFilter filter);
        Task<IOResult<T>> UpdateAsync(T dynamicObject);
        Task<IOResult<bool>> DeleteAsync(long id);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}