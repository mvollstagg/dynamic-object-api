using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IODynamicObject.Application.Interfaces.Services;
using IODynamicObject.Domain.Entities;
using IODynamicObject.Domain.Metadata.Enumeration;
using IODynamicObject.Domain.Metadata.Models;
using IODynamicObject.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using IODynamicObjectEntity = IODynamicObject.Domain.Entities.IODynamicObject;

namespace IODynamicObject.Infrastructure.Services
{
    public class IODynamicObjectService : IIODynamicObjectService
    {
        private readonly IODataContext _context;

        public IODynamicObjectService(IODataContext context)
        {
            _context = context;
        }

        public async Task<IOResult<IODynamicObjectEntity>> CreateAsync(IODynamicObjectEntity dynamicObject)
        {
            try
            {
                dynamicObject.CreationDateUtc = DateTime.UtcNow;
                dynamicObject.ModificationDateUtc = DateTime.UtcNow;

                await _context.IODynamicObjects.AddAsync(dynamicObject);
                await _context.SaveChangesAsync();

                return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Success, dynamicObject);
            }
            catch (Exception ex)
            {
                return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<IODynamicObjectEntity>> GetByIdAsync(long id)
        {
            var dynamicObject = await _context.IODynamicObjects.FindAsync(id);

            if (dynamicObject == null || dynamicObject.Deleted)
            {
                return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Error, "Object not found.");
            }

            return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Success, dynamicObject);
        }

        public async Task<IOResult<List<IODynamicObjectEntity>>> GetByTypeAsync(string objectType)
        {
            var dynamicObjects = await _context.IODynamicObjects
                .Where(o => o.ObjectType == objectType && !o.Deleted)
                .ToListAsync();

            return new IOResult<List<IODynamicObjectEntity>>(IOResultStatusEnum.Success, dynamicObjects);
        }

        public async Task<IOResult<IODynamicObjectEntity>> UpdateAsync(IODynamicObjectEntity dynamicObject)
        {
            try
            {
                var existingObject = await _context.IODynamicObjects.FindAsync(dynamicObject.Id);

                if (existingObject == null || existingObject.Deleted)
                {
                    return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Error, "Object not found.");
                }

                existingObject.Data = dynamicObject.Data;
                existingObject.ObjectType = dynamicObject.ObjectType;
                existingObject.ModificationDateUtc = DateTime.UtcNow;

                _context.IODynamicObjects.Update(existingObject);
                await _context.SaveChangesAsync();

                return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Success, existingObject);
            }
            catch (Exception ex)
            {
                return new IOResult<IODynamicObjectEntity>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<bool>> DeleteAsync(long id)
        {
            var dynamicObject = await _context.IODynamicObjects.FindAsync(id);

            if (dynamicObject == null || dynamicObject.Deleted)
            {
                return new IOResult<bool>(IOResultStatusEnum.Error, false, "Object not found.");
            }

            dynamicObject.Deleted = true;
            dynamicObject.ModificationDateUtc = DateTime.UtcNow;

            _context.IODynamicObjects.Update(dynamicObject);
            await _context.SaveChangesAsync();

            return new IOResult<bool>(IOResultStatusEnum.Success, true);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
