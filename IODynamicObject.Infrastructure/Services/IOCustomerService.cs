using IODynamicObject.Application.Interfaces.Services;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Infrastructure.Persistence;
using IODynamicObject.Application.Types.Customers;
using IODynamicObject.Infrastructure.Filters;
using IODynamicObject.Core.Filtering;

namespace IODynamicObject.Infrastructure.Services
{
    public class IOCustomerService : IIODynamicObjectService<IOCustomer, CustomerFilter>
    {
        private readonly IODataContext _context;

        public IOCustomerService(IODataContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<IOResult<IOCustomer>> CreateAsync(IOCustomer customer)
        {
            try
            {
                customer.CreationDateUtc = DateTime.UtcNow;
                customer.ModificationDateUtc = DateTime.UtcNow;

                // Add customer to the context
                await _context.Customers.AddAsync(customer);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return new IOResult<IOCustomer>(IOResultStatusEnum.Success, customer);
            }
            catch (Exception ex)
            {
                return new IOResult<IOCustomer>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<IOCustomer>> GetByIdAsync(long id)
        {
            try
            {
                // Find customer by ID
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null || customer.Deleted)
                {
                    return new IOResult<IOCustomer>(IOResultStatusEnum.Error, "Customer not found.");
                }

                return new IOResult<IOCustomer>(IOResultStatusEnum.Success, customer);
            }
            catch (Exception ex)
            {
                return new IOResult<IOCustomer>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<List<IOCustomer>>> GetByFiltersAsync(CustomerFilter filter)
        {
            try
            {
                // Retrieve all customers that aren't deleted
                var customers = _context.Customers.Where(c => !c.Deleted);

                // Apply filters here (you would extend this with your own filter logic)
                if (filter != null)
                {
                    customers = IOFilteringHelper<IOCustomer, CustomerFilter, CustomerFilterRule>.ApplyFilter(customers, filter);
                }

                var resultList = await customers.ToListAsync();

                return new IOResult<List<IOCustomer>>(IOResultStatusEnum.Success, resultList);
            }
            catch (Exception ex)
            {
                return new IOResult<List<IOCustomer>>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<IOCustomer>> UpdateAsync(IOCustomer customer)
        {
            try
            {
                var existingCustomer = await _context.Customers.FindAsync(customer.Id);

                if (existingCustomer == null || existingCustomer.Deleted)
                {
                    return new IOResult<IOCustomer>(IOResultStatusEnum.Error, "Customer not found.");
                }

                // Update customer details
                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.Email = customer.Email;
                existingCustomer.Phone = customer.Phone;
                existingCustomer.Address = customer.Address;
                existingCustomer.ModificationDateUtc = DateTime.UtcNow;

                _context.Customers.Update(existingCustomer);
                await _context.SaveChangesAsync();

                return new IOResult<IOCustomer>(IOResultStatusEnum.Success, existingCustomer);
            }
            catch (Exception ex)
            {
                return new IOResult<IOCustomer>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<bool>> DeleteAsync(long id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null || customer.Deleted)
                {
                    return new IOResult<bool>(IOResultStatusEnum.Error, false, "Customer not found.");
                }

                // Mark as deleted
                customer.Deleted = true;
                customer.ModificationDateUtc = DateTime.UtcNow;

                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();

                return new IOResult<bool>(IOResultStatusEnum.Success, true);
            }
            catch (Exception ex)
            {
                return new IOResult<bool>(IOResultStatusEnum.Error, false, ex.Message);
            }
        }
    }
}
