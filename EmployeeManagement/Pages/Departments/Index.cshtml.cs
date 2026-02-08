using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Pages.Departments
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
        {
            _httpClient = httpClientFactory.CreateClient("EmployeeManagementApi");
            _logger = logger;
        }

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