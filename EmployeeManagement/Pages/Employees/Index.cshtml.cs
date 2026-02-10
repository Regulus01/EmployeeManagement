using EmployeeManagement.Web.Clients;
using EmployeeManagement.Web.Entities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Pages.Employees
{
    public class IndexModel : PageModel
    {
        private readonly IDepartmentClient _departmentClient;
        private readonly IEmployeeApiClient _employeeClient;

        public IndexModel(IDepartmentClient departmentClient, IEmployeeApiClient employeeClient)
        {
            _departmentClient = departmentClient;
            _employeeClient = employeeClient;
        }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Nome")]
        public string? Nome { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "CPF")]
        public string? CPF { get; set; }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "RG")]
        public string? RG { get; set; }

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

        public List<GetListEmployeeDto> Employees { get; set; } = new();
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

            var query = new Dictionary<string, string?>
            {
                ["Nome"] = string.IsNullOrWhiteSpace(Nome) ? null : Nome,
                ["CPF"] = string.IsNullOrWhiteSpace(CPF) ? null : CPF,
                ["RG"] = string.IsNullOrWhiteSpace(RG) ? null : RG,
                ["DepartmentId"] = DepartmentId.HasValue ? DepartmentId.Value.ToString() : null,
                ["PageNumber"] = PageNumber.ToString(),
                ["PageSize"] = PageSize.ToString()
            };

            var queryString = string.Join("&",
                query
                    .Where(kv => !string.IsNullOrWhiteSpace(kv.Value))
                    .Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value!)}"));

            var url = string.IsNullOrEmpty(queryString)
                ? "api/Employee"
                : $"api/Employee?{queryString}";

            var response = await _employeeClient.GetEmployeesAsync(cancellationToken);

            if (response is null)
            {
                Employees = [];
                TotalCount = 0;
                return;
            }

            Employees = response.Employees;
            TotalCount = response.TotalCount;
        }

        private async Task LoadDepartmentsAsync(CancellationToken cancellationToken)
        {
            var apiResponse = await _departmentClient.GetDepartmentsAsync(cancellationToken);

            if (apiResponse?.Departments is null)
            {
                Departments = [];
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
    }
}