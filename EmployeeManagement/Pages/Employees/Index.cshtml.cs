using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Pages.Employees
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
        [Display(Name = "Busca (Nome)")]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Departamento")]
        public Guid? DepartmentId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public int TotalCount { get; set; }
        public int TotalPages => PageSize > 0
            ? (int)Math.Ceiling(TotalCount / (double)PageSize)
            : 0;

        public List<EmployeeItem> Employees { get; set; } = new();
        public List<SelectListItem> Departments { get; set; } = new();

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            if (PageNumber <= 0) PageNumber = 1;
            if (PageSize <= 0) PageSize = 10;

            await LoadDepartmentsAsync(cancellationToken);
            await LoadEmployeesAsync(cancellationToken);
        }

        private async Task LoadEmployeesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var query = new Dictionary<string, string?>
                {
                    ["search"] = string.IsNullOrWhiteSpace(Search) ? null : Search,
                    ["departmentId"] = DepartmentId.HasValue ? DepartmentId.Value.ToString() : null,
                    ["pageNumber"] = PageNumber.ToString(),
                    ["pageSize"] = PageSize.ToString()
                };

                var queryString = string.Join("&",
                    query
                        .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                        .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value!)}"));

                var url = string.IsNullOrEmpty(queryString)
                    ? "api/employee"
                    : $"api/employee?{queryString}";

                var response = await _httpClient.GetFromJsonAsync<PagedEmployeesResponse>(
                    url,
                    cancellationToken
                );

                if (response is null)
                {
                    Employees = new List<EmployeeItem>();
                    TotalCount = 0;
                    return;
                }

                Employees = response.Employees.ToList();
                TotalCount = response.TotalCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar employees da API");
                Employees = new List<EmployeeItem>();
                TotalCount = 0;
            }
        }

        private async Task LoadDepartmentsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var apiResponse = await _httpClient.GetFromJsonAsync<GetDepartmentsApiResponse>(
                    "api/Department",
                    cancellationToken
                );

                if (apiResponse?.Departments is null)
                {
                    Departments = new List<SelectListItem>();
                    return;
                }

                Departments = apiResponse.Departments
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Nome
                    })
                    .ToList();

                Departments.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "Todos",
                    Selected = !DepartmentId.HasValue
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar departamentos da API");
                Departments = new List<SelectListItem>();
            }
        }

        public class EmployeeItem
        {
            public Guid Id { get; set; }
            public string Nome { get; set; } = string.Empty;
            public string CPF { get; set; } = string.Empty;
            public string? RG { get; set; }
            public string DepartmentName { get; set; } = string.Empty;
        }

        public class PagedEmployeesResponse
        {
            public IEnumerable<EmployeeItem> Employees { get; set; } = Enumerable.Empty<EmployeeItem>();
            public int TotalCount { get; set; }
        }

        public class GetDepartmentsApiResponse
        {
            public IEnumerable<DepartmentItem> Departments { get; set; } = Enumerable.Empty<DepartmentItem>();
        }

        public class DepartmentItem
        {               
            public Guid Id { get; set; }
            public string Nome { get; set; } = string.Empty;
        }
    }
}       