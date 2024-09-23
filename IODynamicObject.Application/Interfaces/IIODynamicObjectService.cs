using Microsoft.EntityFrameworkCore.Storage;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObjectEntity = IODynamicObject.Domain.Entities.IODynamicObject;
using IODynamicObject.Core.Metadata.Enumeration;

namespace IODynamicObject.Application.Interfaces.Services
{
    public interface IIODynamicObjectService
    {
        Task<IOResult<IODynamicObjectEntity>> CreateAsync(IODynamicObjectEntity dynamicObject);
        Task<IOResult<IODynamicObjectEntity>> GetByGuidAsync(SchemaTypeEnum schemaType, Guid guid);
        Task<IOResult<object>> GetByFiltersAsync(SchemaTypeEnum schemaType, object filter);
        Task<IOResult<IODynamicObjectEntity>> UpdateAsync(IODynamicObjectEntity dynamicObject);
        Task<IOResult<bool>> DeleteAsync(long id);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}