using EmployeeManagement.Application.UseCases.Department.Create;
using EmployeeManagement.Application.UseCases.Department.GetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    /// <summary>
    /// Controlador responsável pelas operações relacionadas a departamentos.
    /// </summary>
    [ApiController]
    [Route("api/Department")]
    public class DepartmentController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="DepartmentController"/>.
        /// </summary>
        /// <param name="mediator">Instância do mediador para envio de comandos e consultas.</param>
        public DepartmentController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Cria um novo departamento.
        /// </summary>
        /// <param name="request">Dados para criação do departamento.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação.</param>
        /// <returns>Retorna os dados do departamento criado.</returns>
        /// <response code="201">Departamento criado com sucesso.</response>
        /// <response code="422">Erro de validação nos dados fornecidos.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreateDepartmentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentRequest request, CancellationToken cancellationToken)
        {
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
            
            return Created(string.Empty, response.Value);
        }

        /// <summary>
        /// Lista departamentos com filtros opcionais.
        /// </summary>
        /// <param name="nome">Filtro por nome do departamento (contém).</param>
        /// <param name="managerName">Filtro por nome do gerente (contém).</param>
        /// <param name="parentDepartmentName">Filtro por nome do departamento superior (contém).</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        [HttpGet]
        [ProducesResponseType(typeof(GetListDepartmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Get(
            [FromQuery] string? nome,
            [FromQuery] string? managerName,
            [FromQuery] string? parentDepartmentName,
            CancellationToken cancellationToken)
        {
            var request = new GetListDepartmentRequest
            {
                Nome = nome,
                ManagerName = managerName,
                ParentDepartmentName = parentDepartmentName
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
