using IODynamicObject.Application.Interfaces.Services;
using IODynamicObject.Core.Metadata.Models;
using IODynamicObject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using IODynamicObject.Core.Metadata.Enumeration;
using IODynamicObject.Infrastructure.Persistence;
using IODynamicObject.Application.Types.Products;
using IODynamicObject.Infrastructure.Filters;
using IODynamicObject.Core.Filtering;

namespace IODynamicObject.Infrastructure.Services
{
    public class IOProductService : IIODynamicObjectService<IOProduct, ProductFilter>
    {
        private readonly IODataContext _context;

        public IOProductService(IODataContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<IOResult<IOProduct>> CreateAsync(IOProduct product)
        {
            try
            {
                product.CreationDateUtc = DateTime.UtcNow;
                product.ModificationDateUtc = DateTime.UtcNow;

                // Process dynamic objects (e.g., specifications)
                if (product.DynamicObjects != null && product.DynamicObjects.Any())
                {
                    foreach (var dynamicObject in product.DynamicObjects)
                    {
                        // Set creation dates for dynamic objects and fields
                        dynamicObject.CreationDateUtc = DateTime.UtcNow;
                        dynamicObject.ModificationDateUtc = DateTime.UtcNow;

                        foreach (var field in dynamicObject.Fields)
                        {
                            field.CreationDateUtc = DateTime.UtcNow;
                            field.ModificationDateUtc = DateTime.UtcNow;

                            foreach (var value in field.Values)
                            {
                                value.CreationDateUtc = DateTime.UtcNow;
                                value.ModificationDateUtc = DateTime.UtcNow;
                            }
                        }
                    }
                }

                // Add product with dynamic objects to the context
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return new IOResult<IOProduct>(IOResultStatusEnum.Success, product);
            }
            catch (Exception ex)
            {
                return new IOResult<IOProduct>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<IOProduct>> GetByIdAsync(long id)
        {
            try
            {
                // Find product by ID and include dynamic objects
                var product = await _context.Products
                    .Include(p => p.DynamicObjects)
                    .ThenInclude(o => o.Fields)
                    .ThenInclude(f => f.Values)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null || product.Deleted)
                {
                    return new IOResult<IOProduct>(IOResultStatusEnum.Error, "Product not found.");
                }

                return new IOResult<IOProduct>(IOResultStatusEnum.Success, product);
            }
            catch (Exception ex)
            {
                return new IOResult<IOProduct>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<List<IOProduct>>> GetByFiltersAsync(ProductFilter filter)
        {
            try
            {
                // Retrieve all products that aren't deleted
                var products = _context.Products.Where(p => !p.Deleted);

                // Apply filters here (you would extend this with your own filter logic)
                if (filter != null)
                {
                    products = IOFilteringHelper<IOProduct, ProductFilter, ProductFilterRule>.ApplyFilter(products, filter);
                }

                var resultList = await products.ToListAsync();

                return new IOResult<List<IOProduct>>(IOResultStatusEnum.Success, resultList);
            }
            catch (Exception ex)
            {
                return new IOResult<List<IOProduct>>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<IOProduct>> UpdateAsync(IOProduct product)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(product.Id);

                if (existingProduct == null || existingProduct.Deleted)
                {
                    return new IOResult<IOProduct>(IOResultStatusEnum.Error, "Product not found.");
                }

                // Update product details
                existingProduct.Name = product.Name;
                existingProduct.Brand = product.Brand;
                existingProduct.Price = product.Price;
                existingProduct.Category = product.Category;
                existingProduct.ModificationDateUtc = DateTime.UtcNow;

                _context.Products.Update(existingProduct);
                await _context.SaveChangesAsync();

                return new IOResult<IOProduct>(IOResultStatusEnum.Success, existingProduct);
            }
            catch (Exception ex)
            {
                return new IOResult<IOProduct>(IOResultStatusEnum.Error, ex.Message);
            }
        }

        public async Task<IOResult<bool>> DeleteAsync(long id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null || product.Deleted)
                {
                    return new IOResult<bool>(IOResultStatusEnum.Error, false, "Product not found.");
                }

                // Mark as deleted
                product.Deleted = true;
                product.ModificationDateUtc = DateTime.UtcNow;

                _context.Products.Update(product);
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
