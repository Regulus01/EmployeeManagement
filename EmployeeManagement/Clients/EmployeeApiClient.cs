using EmployeeManagement.Web.Entities.Request;
using EmployeeManagement.Web.Entities.Response;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Web.Clients
{
    public class EmployeeApiClient : IEmployeeApiClient
    {
        private readonly HttpClient _httpClient;

        public EmployeeApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetListEmployeeResponse?> GetEmployeesAsync(string? query = null, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetFromJsonAsync<GetListEmployeeResponse>($"api/Employee?{query}", cancellationToken);
            return response;
        }

        public async Task<ApiResponse> CreateAsync(CreateEmployeeApiRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Employee", request, cancellationToken);

            if (response.IsSuccessStatusCode)
                return ApiResponse.Ok();

            var errors = await ExtractErrors(response, cancellationToken);
            return ApiResponse.Fail(errors);
        }

        private static async Task<List<string>> ExtractErrors(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            try
            {
                var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(cancellationToken: cancellationToken);

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
