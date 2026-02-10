using EmployeeManagement.Web.Clients;
using EmployeeManagement.Web.Entities.Request;
using EmployeeManagement.Web.Inputs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EmployeeManagement.Pages.Employees
{
    public class CreateModel : PageModel
    {
        private readonly IDepartmentClient _departmentClient;
        private readonly IEmployeeApiClient _employeeClient;

        public CreateModel(IDepartmentClient departmentClient, IEmployeeApiClient employeeClient)
        {
            _departmentClient = departmentClient;
            _employeeClient = employeeClient;
        }

        [BindProperty]
        public CreateEmployeeInputModel Input { get; set; } = new();

        public List<SelectListItem> Departments { get; set; } = [];

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            await LoadDepartmentsAsync(cancellationToken);
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartmentsAsync(cancellationToken);
                return Page();
            }

            var request = new CreateEmployeeApiRequest
            {
                Nome = Input.Nome,
                CPF = Input.CPF,
                RG = Input.RG,
                DepartmentId = Input.DepartmentId
            };

            var response = await _employeeClient.CreateAsync(request, cancellationToken);

            if (!response.Success)
            {
                await LoadDepartmentsAsync(cancellationToken);
                return Page();
            }

            return RedirectToPage("/Employees/Index");
        }

        private async Task LoadDepartmentsAsync(CancellationToken cancellationToken)
        {
            var response = await _departmentClient.GetDepartmentsAsync(cancellationToken);

            if (response?.Departments is null)
            {
                Departments = [];
                return;
            }

            Departments = response.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Nome
                })
                .ToList();
        }
    }
}