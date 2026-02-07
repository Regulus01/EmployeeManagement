namespace EmployeeManagement.Application.UseCases.Employee.Create
{
    public sealed class CreateEmployeeResponse
    {
        public Guid Id { get; init; }
        public string Nome { get; init; } = string.Empty;
        public string CPF { get; init; } = string.Empty;
        public string? RG { get; init; }
        public Guid DepartmentId { get; init; }
    }
}
