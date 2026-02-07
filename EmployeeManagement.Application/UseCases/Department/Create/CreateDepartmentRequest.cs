namespace EmployeeManagement.Application.UseCases.Department.Create
{
    internal sealed class CreateDepartmentRequest
    {
        public string Nome { get; init; } = null!;
        public Guid? ManagerId { get; init; }
        public Guid? ParentDepartmentId { get; init; }
    }
}
