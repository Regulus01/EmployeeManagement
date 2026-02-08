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
        private readonly HttpClient _httpClient;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(IHttpClientFactory httpClientFactory, ILogger<CreateModel> logger)
        {
            _httpClient = httpClientFactory.CreateClient("EmployeeManagementApi");
            _logger = logger;
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

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await LoadParentDepartmentsAsync(cancellationToken);
                await LoadManagersAsync(cancellationToken);
                return Page();
            }

            try
            {
                var apiRequest = new CreateDepartmentApiRequest
                {
                    Nome = Input.Nome,
                    ManagerId = Input.ManagerId,
                    ParentDepartmentId = Input.ParentDepartmentId
                };

                var response = await _httpClient.PostAsJsonAsync("api/Department", apiRequest, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogWarning("Erro ao criar departamento na API. Status {Status}, Body: {Body}",
                        response.StatusCode, errorBody);

                    try
                    {
                        var errorObj = JsonSerializer.Deserialize<ApiErrorResponse>(errorBody,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (errorObj != null)
                        {
                            if (errorObj.Errors != null && errorObj.Errors.Count > 0)
                            {
                                foreach (var kv in errorObj.Errors)
                                {
                                    foreach (var message in kv.Value)
                                    {
                                        ModelState.AddModelError(string.Empty, message);
                                    }
                                }
                            }
                            else if (!string.IsNullOrWhiteSpace(errorObj.Detail))
                            {
                                ModelState.AddModelError(string.Empty, errorObj.Detail);
                            }
                            else if (!string.IsNullOrWhiteSpace(errorObj.Title))
                            {
                                ModelState.AddModelError(string.Empty, errorObj.Title);
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Falha ao criar departamento na API.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Falha ao criar departamento na API.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Falha ao desserializar corpo de erro da API de departments");
                        ModelState.AddModelError(string.Empty, "Falha ao criar departamento na API.");
                    }

                    await LoadParentDepartmentsAsync(cancellationToken);
                    await LoadManagersAsync(cancellationToken);
                    return Page();
                }

                return RedirectToPage("/Departments/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao chamar API de departments");
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao processar sua requisição.");
                await LoadParentDepartmentsAsync(cancellationToken);
                await LoadManagersAsync(cancellationToken);
                return Page();
            }
        }

        private async Task LoadParentDepartmentsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GetListDepartmentResponse>(
                    "api/Department",
                    cancellationToken
                );

                var departments = response?.Departments ?? new List<GetListDepartmentDto>();

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
                    Text = "Nenhum (raiz)",
                    Selected = !Input.ParentDepartmentId.HasValue
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar departamentos pais da API");
                ParentDepartments = new List<SelectListItem>();
            }
        }

        private async Task LoadManagersAsync(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GetListEmployeeResponse>(
                    "api/Employee",
                    cancellationToken
                );

                var employees = response?.Employees ?? new List<GetListEmployeeDto>();

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
                Managers = new List<SelectListItem>();
            }
        }

        public class CreateDepartmentInputModel
        {
            [Required]
            [Display(Name = "Nome")]
            public string Nome { get; set; } = string.Empty;

            [Display(Name = "Gerente")]
            public Guid? ManagerId { get; set; }

            [Display(Name = "Departamento Pai")]
            public Guid? ParentDepartmentId { get; set; }
        }

        public class CreateDepartmentApiRequest
        {
            public string Nome { get; set; } = string.Empty;
            public Guid? ManagerId { get; set; }
            public Guid? ParentDepartmentId { get; set; }
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

        public class GetListEmployeeResponse
        {
            public List<GetListEmployeeDto> Employees { get; set; } = new();
            public int TotalCount { get; set; }
        }

        public class GetListEmployeeDto
        {
            public Guid Id { get; set; }
            public string Nome { get; set; } = string.Empty;
        }

        public class ApiErrorResponse
        {
            public string? Title { get; set; }
            public int Status { get; set; }
            public string? Detail { get; set; }
            public Dictionary<string, string[]> Errors { get; set; } = new();
        }
    }
}