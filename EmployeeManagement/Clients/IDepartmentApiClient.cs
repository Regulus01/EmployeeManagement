using EmployeeManagement.Web.Entities.Request;
using EmployeeManagement.Web.Entities.Response;

namespace EmployeeManagement.Web.Clients
{
    public interface IDepartmentClient
    {
        Task<ApiResponse> CreateAsync(CreateDepartmentRequest request, CancellationToken cancelationToken);
        Task<GetListDepartmentResponse?> GetDepartmentsAsync(CancellationToken cancelationToken);
    }
}
