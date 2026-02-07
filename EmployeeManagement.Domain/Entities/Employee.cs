namespace EmployeeManagement.Domain.Entities
{
    internal class Employee : BaseEntity
    {
        public string Nome { get; private set; }
        public required string CPF { get; init; }
        public string? RG { get; private set; }
        public Guid DepartmentId { get; private set; }
    }
}
