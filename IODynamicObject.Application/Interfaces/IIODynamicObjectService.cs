using Microsoft.EntityFrameworkCore.Storage;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObjectEntity = IODynamicObject.Domain.Entities.IODynamicObject;

namespace IODynamicObject.Application.Interfaces.Services
{
    public interface IIODynamicObjectService
    {
        Task<IOResult<IODynamicObjectEntity>> CreateAsync(IODynamicObjectEntity dynamicObject);
        Task<IOResult<IODynamicObjectEntity>> GetByIdAsync(long id);
        Task<IOResult<List<IODynamicObjectEntity>>> GetByTypeAsync(string objectType);
        Task<IOResult<List<IODynamicObjectEntity>>> GetByTypeAndFiltersAsync(string objectType,
                                                                            Dictionary<string, string> filters,
                                                                            int pageNumber,
                                                                            int pageSize,
                                                                            string sortBy,
                                                                            string sortOrder);
        Task<IOResult<IODynamicObjectEntity>> UpdateAsync(IODynamicObjectEntity dynamicObject);
        Task<IOResult<bool>> DeleteAsync(long id);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}