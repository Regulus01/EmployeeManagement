using EmployeeManagement.Web.Entities.Request;
using EmployeeManagement.Web.Entities.Response;

namespace EmployeeManagement.Web.Clients
{
    public interface IDepartmentClient
    {
        Task<ApiResponse> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken);
        Task<GetListDepartmentResponse?> GetDepartmentsAsync(string? query = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<GetSubDepartmentsResponse>> GetSubDepartmentsAsync(Guid departmentId, CancellationToken cancellationToken);
    }
}
