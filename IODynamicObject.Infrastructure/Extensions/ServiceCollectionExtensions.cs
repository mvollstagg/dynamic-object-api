using IODynamicObject.Application.Interfaces.Services;
using IODynamicObject.Application.Validators;
using IODynamicObject.Application.Validators.Implementations;
using IODynamicObject.Infrastructure.Persistence;
using IODynamicObject.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IODynamicObject.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IODataContext>(options =>
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 28))));

            services.AddScoped<IOCustomerService>();
            services.AddScoped<IOProductService>();
            services.AddScoped<IOOrderService>();
            services.AddScoped<IIODynamicObjectValidator, IODynamicObjectValidator>();

            return services;
        }
    }
}
