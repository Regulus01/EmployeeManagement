using EmployeeManagement.Web.Clients;
using EmployeeManagement.Web.Entities.Request;
using EmployeeManagement.Web.Inputs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;

namespace EmployeeManagement.Pages.Departments
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
        public CreateDepartmentInputModel Input { get; set; } = new();

        public List<SelectListItem> ParentDepartments { get; set; } = [];
        public List<SelectListItem> Managers { get; set; } = [];

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            await LoadParentDepartmentsAsync(cancellationToken);
            await LoadManagersAsync(cancellationToken);
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancelationToken)
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(cancelationToken);
                return Page();
            }

            var result = await _departmentClient.CreateAsync(new CreateDepartmentRequest
            {
                Nome = Input.Nome,
                ManagerId = Input.ManagerId,
                ParentDepartmentId = Input.ParentDepartmentId
            }, cancelationToken);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                await OnGetAsync(cancelationToken);
                return Page();
            }

            return RedirectToPage("/Departments/Index");
        }

        private async Task LoadParentDepartmentsAsync(CancellationToken cancellationToken)
        {
            var response = await _departmentClient.GetDepartmentsAsync(cancellationToken: cancellationToken);

            if (response == null)
                return;

            ParentDepartments = response.Departments
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Nome
                })
                .ToList();

            ParentDepartments.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Nenhum",
                Selected = !Input.ParentDepartmentId.HasValue
            });
        }

        private async Task LoadManagersAsync(CancellationToken cancellationToken)
        {
            var response = await _employeeClient.GetEmployeesAsync(cancellationToken: cancellationToken);

            if (response == null)
                return;

            Managers = response.Employees
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Nome
                })
                .ToList();

            Managers.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Selecione um gerente",
                Selected = !Input.ManagerId.HasValue
            });

        }
    }
}