using EmployeeManagement.Application.UseCases.Employee.Create;
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
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddScoped<CreateEmployeeUseCase>();

            return services;
        }
    }
}
