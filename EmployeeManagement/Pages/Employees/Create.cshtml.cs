using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EmployeeManagement.Pages.Employees
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
        public CreateEmployeeInputModel Input { get; set; } = new();

        public List<SelectListItem> Departments { get; set; } = new();

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

            try
            {
                var apiRequest = new CreateEmployeeApiRequest
                {
                    Nome = Input.Nome,
                    CPF = Input.CPF,
                    RG = Input.RG,
                    DepartmentId = Input.DepartmentId
                };

                var response = await _httpClient.PostAsJsonAsync("api/employee", apiRequest, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogWarning("Erro ao criar employee na API. Status {Status}, Body: {Body}",
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
                                ModelState.AddModelError(string.Empty, "Falha ao criar colaborador na API.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Falha ao criar colaborador na API.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Falha ao desserializar corpo de erro da API de employees");
                        ModelState.AddModelError(string.Empty, "Falha ao criar colaborador na API.");
                    }

                    await LoadDepartmentsAsync(cancellationToken);
                    return Page();
                }

                return RedirectToPage("/Employees/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao chamar API de employees");
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao processar sua requisição.");
                await LoadDepartmentsAsync(cancellationToken);
                return Page();
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar departamentos da API");
                Departments = new List<SelectListItem>();
            }
        }

        public class CreateEmployeeInputModel
        {
            [Required]
            [Display(Name = "Nome")]
            public string Nome { get; set; } = string.Empty;

            [Required]
            [Display(Name = "CPF")]
            public string CPF { get; set; } = string.Empty;

            [Display(Name = "RG")]
            public string? RG { get; set; }

            [Required]
            [Display(Name = "Departamento")]
            public Guid DepartmentId { get; set; }
        }

        public class CreateEmployeeApiRequest
        {
            public string Nome { get; set; } = string.Empty;
            public string CPF { get; set; } = string.Empty;
            public string? RG { get; set; }
            public Guid DepartmentId { get; set; }
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

        public class ApiErrorResponse
        {
            public string? Title { get; set; }
            public int Status { get; set; }
            public string? Detail { get; set; }
            public Dictionary<string, string[]> Errors { get; set; } = new();
        }
    }
}