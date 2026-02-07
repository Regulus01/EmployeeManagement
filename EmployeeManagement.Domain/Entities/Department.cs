namespace EmployeeManagement.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Nome { get; private set; }
        public Guid? ManagerId { get; private set; }
        public Employee? Manager { get; private set; }
        public Guid? ParentDepartmentId { get; private set; }
        public Department? ParentDepartment { get; private set; }
        public virtual ICollection<Employee> Employees { get; private set; } = [];
        public virtual ICollection<Department> SubDepartments { get; private set; } = [];

        public Department(string nome, Guid? managerId = null, Guid? parentDepartmentId = null)
        {
            Nome = nome;
            ManagerId = managerId;
            ParentDepartmentId = parentDepartmentId;
        }
    }
}