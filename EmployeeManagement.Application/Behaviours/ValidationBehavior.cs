using EmployeeManagement.Application.Common;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace EmployeeManagement.Application.Behaviors
{
    /// <summary>
    /// Behavior de pipeline do MediatR responsável por executar automaticamente
    /// as validações do FluentValidation antes da execução do handler da requisição.
    /// </summary>
    /// <typeparam name="TRequest">Tipo da requisição.</typeparam>
    /// <typeparam name="TResponse">Tipo da resposta retornada pelo handler.</typeparam>
    internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;


        /// <summary>
        /// Construtor do behavior de validação.
        /// </summary>
        /// <param name="validators">
        /// Conjunto de validadores do FluentValidation registrados para o tipo de requisição.
        /// </param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? Array.Empty<IValidator<TRequest>>();
        }

        /// <summary>
        /// Executa o pipeline de validação antes da execução do handler.
        /// </summary>
        /// <param name="request">Requisição sendo processada.</param>
        /// <param name="next">Delegate que executa o próximo passo do pipeline (handler).</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>
        /// A resposta do handler caso a validação seja bem-sucedida,
        /// ou uma resposta de falha de validação.
        /// </returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationTasks = _validators
                    .Select(v => v.ValidateAsync(context, cancellationToken));

                var results = await Task.WhenAll(validationTasks);

                var failures = results
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Any())
                {
                    var errors = failures.Select(f => f.ErrorMessage).ToArray();

                    return CreateValidationFailureResponse(errors);
                }
            }

            return await next();
        }

        /// <summary>
        /// Cria uma resposta de falha de validação.
        /// </summary>
        /// <param name="errors">Lista de mensagens de erro.</param>
        /// <returns>
        /// Um <see cref="Result{T}"/> de falha quando o tipo de resposta for compatível.
        /// Caso contrário, lança uma <see cref="ValidationException"/>.
        /// </returns>
        private static TResponse CreateValidationFailureResponse(string[] errors)
        {
            var responseType = typeof(TResponse);

            if (responseType.IsGenericType &&
                responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var failureMethod = typeof(Result)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(m => m.Name == nameof(Result.Failure) && m.IsGenericMethod)
                    .FirstOrDefault()
                    ?.MakeGenericMethod(responseType.GetGenericArguments()[0]);

                if (failureMethod != null)
                {
                    return (TResponse)failureMethod.Invoke(null, new object[] { errors })!;
                }
            }

            throw new ValidationException(string.Join("; ", errors));
        }
    }
}