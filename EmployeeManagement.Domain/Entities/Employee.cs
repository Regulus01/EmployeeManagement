namespace EmployeeManagement.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public string? RG { get; private set; }
        public Guid DepartmentId { get; private set; }
        public virtual Department Department { get; private set; }

        public Employee()
        {
        }

        public Employee(string nome, string cpf, string? rg, Guid departmentId)
        {
            Nome = nome;
            CPF = cpf;
            RG = rg;
            DepartmentId = departmentId;
        }
    }
}
