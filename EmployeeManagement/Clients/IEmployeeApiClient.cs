using EmployeeManagement.Web.Entities.Request;
using EmployeeManagement.Web.Entities.Response;

namespace EmployeeManagement.Web.Clients
{
    public interface IEmployeeApiClient
    {
        Task<ApiResponse> CreateAsync(CreateEmployeeApiRequest request, CancellationToken cancelationToken);
        Task<GetListEmployeeResponse?> GetEmployeesAsync(CancellationToken cancelationToken);
    }
}
