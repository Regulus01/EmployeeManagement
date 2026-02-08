using EmployeeManagement.Application.UseCases.Employee.Create;
using EmployeeManagement.Application.UseCases.Employee.GetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    /// <summary>
    /// Controlador responsável pelas operações relacionadas a funcionários.
    /// </summary>
    [ApiController]
    [Route("api/Employee")]
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


        /// <summary>
        /// Lista colaboradores com filtros opcionais.
        /// </summary>
        /// <param name="nome">Filtro por nome do colaborador (busca parcial, case-insensitive).</param>
        /// <param name="cpf">Filtro por CPF do colaborador (busca exata).</param>
        /// <param name="rg">Filtro por RG do colaborador (busca parcial).</param>
        /// <param name="departmentId">Filtro por ID do departamento.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação.</param>
        /// <returns>Lista de colaboradores filtrados.</returns>
        /// <response code="200">Lista de colaboradores retornada com sucesso.</response>
        /// <response code="422">Erro de validação nos filtros fornecidos.</response>
        [HttpGet]
        [ProducesResponseType(typeof(GetListEmployeeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> GetListEmployees(
            [FromQuery] string? nome,
            [FromQuery] string? cpf,
            [FromQuery] string? rg,
            [FromQuery] Guid? departmentId,
            CancellationToken cancellationToken)
        {
            var request = new GetListEmployeeRequest
            {
                Nome = nome,
                CPF = cpf,
                RG = rg,
                DepartmentId = departmentId
            };

            var response = await _mediator.Send(request, cancellationToken);

            if (!response.IsSuccess)
            {
                var errors = new Dictionary<string, string[]>
                {
                    { string.Empty, response.Errors }
                };

                var problemDetails = new ValidationProblemDetails(errors)
                {
                    Title = "Validation Failed",
                    Detail = "One or more validation errors occurred.",
                    Status = StatusCodes.Status422UnprocessableEntity
                };

                return UnprocessableEntity(problemDetails);
            }

            return Ok(response.Value);
        }
    }
}
