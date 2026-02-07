using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace EmployeeManagement.Application.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra serviços e repositórios da camada Infrastructure.
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
