using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LeaveManagementSystem.Data
{
    public static class DataServicesRegistration
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration
                .GetConnectionString("LeaveManagementSystemWebContextConnection") ?? throw new InvalidOperationException("Connection string 'LeaveManagementSystemWebContextConnection' not found.");

            services.AddDbContext<LeaveManagementSystemWebContext>(options => options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
