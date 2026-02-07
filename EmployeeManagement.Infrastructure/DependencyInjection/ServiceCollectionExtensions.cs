using EmployeeManagement.Domain.Repositories;
using EmployeeManagement.Infrastructure.Context;
using EmployeeManagement.Infrastructure.Repositories;
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
            // Repositórios
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            //Database
            services.AddDbContext<AppDbContext>(options =>
                   options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}