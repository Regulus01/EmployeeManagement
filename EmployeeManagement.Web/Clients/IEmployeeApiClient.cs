using EmployeeManagement.Web.Entities.Request;
using EmployeeManagement.Web.Entities.Response;

namespace EmployeeManagement.Web.Clients
{
    public interface IEmployeeApiClient
    {
        Task<ApiResponse> CreateAsync(CreateEmployeeApiRequest request, CancellationToken cancellationToken);
        Task<GetListEmployeeResponse?> GetEmployeesAsync(string? query = null, CancellationToken cancellationToken = default);
    }
}
