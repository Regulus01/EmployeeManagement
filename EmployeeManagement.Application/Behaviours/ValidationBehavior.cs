using EmployeeManagement.Application.Common;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace EmployeeManagement.Application.Behaviors
{
    internal sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? Array.Empty<IValidator<TRequest>>();
        }

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