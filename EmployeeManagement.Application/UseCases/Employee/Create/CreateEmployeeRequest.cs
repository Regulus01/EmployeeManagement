namespace EmployeeManagement.Application.UseCases.Employee.Create
{
    internal class CreateEmployeeRequest
    {
        public string Nome { get; init; } = string.Empty;
        public string CPF { get; init; } = string.Empty;
        public string? RG { get; init; }
        public Guid DepartmentId { get; init; }
    }
}
