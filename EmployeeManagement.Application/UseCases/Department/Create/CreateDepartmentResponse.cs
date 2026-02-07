namespace EmployeeManagement.Application.UseCases.Department.Create
{
    public sealed class CreateDepartmentResponse
    {
        public Guid Id { get; init; }
        public string Nome { get; init; } = null!;
        public Guid? ManagerId { get; init; }
        public Guid? ParentDepartmentId { get; init; }
    }
}
    