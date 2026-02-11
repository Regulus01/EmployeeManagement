using EmployeeManagement.Application.Common;
using MediatR;

namespace EmployeeManagement.Application.UseCases.Department.GetSubDepartments
{
    public class GetSubdepartmentsRequest : IRequest<Result<List<GetSubdepartmentsResponse>>>
    {
        /// <summary>
        /// ID do departamento.
        /// </summary>
        public Guid DepartmentId { get; set; }
    }
}
