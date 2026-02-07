using MediatR;

namespace EmployeeManagement.Application.UseCases.Employee.Create
{
    public class CreateEmployeeRequest : IRequest<CreateEmployeeResponse>
    {
        public string Nome { get; init; } = string.Empty;
        public string CPF { get; init; } = string.Empty;
        public string? RG { get; init; }
        public Guid DepartmentId { get; init; }
    }
}
