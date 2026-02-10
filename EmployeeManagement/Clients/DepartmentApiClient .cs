using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Web.Entities.Request;
using EmployeeManagement.Web.Entities.Response;

namespace EmployeeManagement.Web.Clients
{
    public class DepartmentApiClient : IDepartmentClient
    {
        private readonly HttpClient _httpClient;

        public DepartmentApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse> CreateAsync(CreateDepartmentRequest request, CancellationToken ct)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Department", request, ct);

            if (response.IsSuccessStatusCode)
                return ApiResponse.Ok();

            var errors = await ExtractErrors(response, ct);
            return ApiResponse.Fail(errors);
        }

        public async Task<GetListDepartmentResponse?> GetDepartmentsAsync(CancellationToken cancelationToken)
        {
            var response = await _httpClient.GetFromJsonAsync<GetListDepartmentResponse>("api/Department", cancelationToken);

            return response;
        }

        public async Task<GetListEmployeeResponse?> GetEmployeesAsync(CancellationToken cancelationToken)
        {
            var response = await _httpClient.GetFromJsonAsync<GetListEmployeeResponse>("api/Employee", cancelationToken);
            return response;
        }

        private static async Task<List<string>> ExtractErrors(HttpResponseMessage response, CancellationToken ct)
        {
            try
            {
                var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(cancellationToken: ct);

                if (problem?.Errors != null)
                    return problem.Errors.SelectMany(x => x.Value).ToList();

                if (!string.IsNullOrWhiteSpace(problem?.Detail))
                    return [problem.Detail];

                return ["Erro na requisição"];
            }
            catch
            {
                return ["Erro na requisição"];
            }
        }
    }
}
