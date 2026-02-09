using EmployeeManagement.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace EmployeeManagement.Application.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registra serviços da camada de Application.
        /// </summary>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddValidators(assembly)
                    .AddMediatRWithPipelines(assembly);

            return services;
        }

        /// <summary>
        /// Registra automaticamente todos os validadores do FluentValidation
        /// presentes no assembly informado.
        /// </summary>
        /// <param name="services">Coleção de serviços da aplicação.</param>
        /// <param name="assembly">Assembly onde os validadores estão localizados.</param>
        /// <returns>A própria coleção de serviços para encadeamento de chamadas.</returns>
        private static IServiceCollection AddValidators(this IServiceCollection services, Assembly assembly)
        {
            services.AddValidatorsFromAssembly(assembly);
            return services;
        }

        /// <summary>
        /// Configura o MediatR e registra os behaviors de pipeline utilizados pela aplicação,
        /// como o comportamento de validação.
        /// </summary>
        /// <param name="services">Coleção de serviços da aplicação.</param>
        /// <param name="assembly">Assembly onde estão os handlers e requests do MediatR.</param>
        /// <returns>A própria coleção de serviços para encadeamento de chamadas.</returns>
        private static IServiceCollection AddMediatRWithPipelines(this IServiceCollection services, Assembly assembly)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
