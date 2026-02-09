using EmployeeManagement.Domain.Repositories;
using EmployeeManagement.Infrastructure.Context;
using EmployeeManagement.Infrastructure.Repositories;
using Humanizer.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Infrastructure.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra serviços e repositórios da camada Infrastructure.
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration)
                    .AddRepositories();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                   options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            return services;
        }
    }
}