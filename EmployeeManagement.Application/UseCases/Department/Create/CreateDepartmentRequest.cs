using EmployeeManagement.Application.Common;
using MediatR;

namespace EmployeeManagement.Application.UseCases.Department.Create
{
    public sealed class CreateDepartmentRequest : IRequest<Result<CreateDepartmentResponse>>
    {
        public string Nome { get; init; } = null!;
        public Guid? ManagerId { get; init; }
        public Guid? ParentDepartmentId { get; init; }
    }
}
