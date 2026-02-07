namespace EmployeeManagement.Domain.Entities
{
    internal class Department : BaseEntity
    {
        public string Nome { get; private set; }
        public Guid? ManagerId { get; private set; }
        public Employee? Manager { get; private set; }
        public Guid? ParentDepartmentId { get; private set; }
        public Department? ParentDepartment { get; private set; }
        public virtual ICollection<Employee> Employees { get; private set; } = [];
        public virtual ICollection<Department> SubDepartments { get; private set; } = [];
    }
}