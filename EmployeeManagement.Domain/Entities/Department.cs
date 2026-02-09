namespace EmployeeManagement.Domain.Entities
{
    public class Department : BaseEntity
    {
        /// <summary>
        /// Nome do departamento.
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Identificador do colaborador que atua como gestor do departamento (opcional).
        /// </summary>
        public Guid? ManagerId { get; private set; }

        /// <summary>
        /// Entidade do colaborador que atua como gestor do departamento.
        /// Navegação opcional.
        /// </summary>
        public Employee? Manager { get; private set; }

        /// <summary>
        /// Identificador do departamento pai na hierarquia (opcional).
        /// </summary>
        public Guid? ParentDepartmentId { get; private set; }

        /// <summary>
        /// Entidade do departamento pai na hierarquia organizacional.
        /// Navegação opcional.
        /// </summary>
        public Department? ParentDepartment { get; private set; }

        /// <summary>
        /// Coleção de colaboradores pertencentes a este departamento.
        /// </summary>
        public virtual ICollection<Employee> Employees { get; private set; } = [];

        /// <summary>
        /// Coleção de subdepartamentos vinculados a este departamento.
        /// Representa a hierarquia organizacional.
        /// </summary>
        public virtual ICollection<Department> SubDepartments { get; private set; } = [];


        public Department(string nome, Guid? managerId = null, Guid? parentDepartmentId = null)
        {
            Nome = nome;
            ManagerId = managerId;
            ParentDepartmentId = parentDepartmentId;
        }
    }
}