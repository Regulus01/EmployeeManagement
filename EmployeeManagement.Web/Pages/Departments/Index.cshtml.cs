using EmployeeManagement.Web.Clients;
using EmployeeManagement.Web.Entities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Pages.Departments
{
    public class IndexModel : PageModel
    {
        private readonly IDepartmentClient _client;

        public IndexModel(IDepartmentClient client)
        {
            _client = client;
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

            var response = await _client.GetDepartmentsAsync(queryString, cancellationToken);

            Departments = response?.Departments ?? [];
            TotalCount = response?.TotalCount ?? 0;
        }
    }
}