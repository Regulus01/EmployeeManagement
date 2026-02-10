using EmployeeManagement.Application.UseCases.Department.Create;
using EmployeeManagement.Application.UseCases.Department.GetList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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

    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
        {
            _httpClient = httpClientFactory.CreateClient("EmployeeManagementApi");
            _logger = logger;
        }

        // Filtros (seguindo assinatura do endpoint)
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Nome do departamento")]
        public string? Nome { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Nome do gerente")]
        public string? ManagerName { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Departamento superior")]
        public string? ParentDepartmentName { get; set; }

        public List<GetListDepartmentDto> Departments { get; set; } = new();
        public int TotalCount { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            try
            {
                var queryParams = new Dictionary<string, string?>
                {
                    ["nome"] = string.IsNullOrWhiteSpace(Nome) ? null : Nome,
                    ["managerName"] = string.IsNullOrWhiteSpace(ManagerName) ? null : ManagerName,
                    ["parentDepartmentName"] = string.IsNullOrWhiteSpace(ParentDepartmentName) ? null : ParentDepartmentName
                };

                var queryString = string.Join("&",
                    queryParams
                        .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                        .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value!)}"));

                var url = string.IsNullOrEmpty(queryString)
                    ? "api/Department"
                    : $"api/Department?{queryString}";

                var response = await _httpClient.GetFromJsonAsync<GetListDepartmentResponse>(
                    url,
                    cancellationToken
                );

                Departments = response?.Departments ?? new List<GetListDepartmentDto>();
                TotalCount = response?.TotalCount ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar lista de departamentos da API");
                Departments = new List<GetListDepartmentDto>();
                TotalCount = 0;
            }
        }
    }

    public class GetListDepartmentResponse
    {
        public List<GetListDepartmentDto> Departments { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class GetListDepartmentDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? ManagerName { get; set; }
        public string? ParentDepartmentName { get; set; }
    }
}
