using EmployeeManagement.Web.Entities.Request;
using EmployeeManagement.Web.Entities.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace EmployeeManagement.Web.Clients
{
    public class EmployeeApiClient : IEmployeeApiClient
    {
        private readonly HttpClient _httpClient;

        public EmployeeApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetListEmployeeResponse?> GetEmployeesAsync(CancellationToken cancelationToken)
        {
            var response = await _httpClient.GetFromJsonAsync<GetListEmployeeResponse>("api/Employee", cancelationToken);
            return response;
        }

        public async Task<ApiResponse> CreateAsync(CreateEmployeeApiRequest request, CancellationToken cancelationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Employee", request, cancelationToken);

            if (response.IsSuccessStatusCode)
                return ApiResponse.Ok();

            var errors = await ExtractErrors(response, cancelationToken);
            return ApiResponse.Fail(errors);
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
