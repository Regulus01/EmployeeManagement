using EmployeeManagement.Application.UseCases.Employee.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    /// <summary>
    /// Controlador responsável pelas operações relacionadas a funcionários.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="EmployeeController"/>.
        /// </summary>
        /// <param name="mediator">Instância do mediador para envio de comandos e consultas.</param>
        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Cria um novo funcionário.
        /// </summary>
        /// <param name="request">Dados para criação do funcionário.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação.</param>
        /// <returns>Retorna os dados do funcionário criado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateEmployeeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);

            if (!response.IsSuccess)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { "ValidationErrors", response.Errors }
                };

                var problemDetails = new ValidationProblemDetails(errors)
                {
                    Title = "Validation Failed",
                    Detail = "One or more validation errors occurred.",
                    Status = StatusCodes.Status422UnprocessableEntity
                };

                return UnprocessableEntity(problemDetails);
            }

            return Created(string.Empty, response.Value);
        }
    }
}
