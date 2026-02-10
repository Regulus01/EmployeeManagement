using EmployeeManagement.Web.Clients;
using EmployeeManagement.Web.Entities.Request;
using EmployeeManagement.Web.Inputs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;

namespace EmployeeManagement.Pages.Departments
{
    public class CreateModel : PageModel
    {
        private readonly IDepartmentClient _client;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(IDepartmentClient client)
        {
            _client = client;
        }

        [BindProperty]
        public CreateDepartmentInputModel Input { get; set; } = new();

        public List<SelectListItem> ParentDepartments { get; set; } = new();
        public List<SelectListItem> Managers { get; set; } = new();

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

            var result = await _client.CreateAsync(new CreateDepartmentRequest
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
            try
            {
                var departments = await _client.GetDepartmentsAsync(cancellationToken);

                ParentDepartments = departments
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar departamentos pais da API");
                ParentDepartments = [];
            }
        }

        private async Task LoadManagersAsync(CancellationToken cancellationToken)
        {
            try
            {
                var employees = await _client.GetEmployeesAsync(cancellationToken);

                Managers = employees
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar colaboradores (managers) da API");
                Managers = [];
            }
        }
    }
}