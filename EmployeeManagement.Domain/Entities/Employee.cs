namespace EmployeeManagement.Domain.Entities
{
    public class Employee : BaseEntity
    {
        /// <summary>
        /// Nome completo do colaborador.
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// CPF do colaborador.
        /// </summary>
        public string CPF { get; private set; }

        /// <summary>
        /// RG do colaborador (opcional).
        /// </summary>
        public string? RG { get; private set; }

        /// <summary>
        /// Identificador do departamento ao qual o colaborador pertence.
        /// </summary>
        public Guid DepartmentId { get; private set; }

        /// <summary>
        /// Departamento ao qual o colaborador está vinculado.
        /// Propriedade de navegação do EF.
        /// </summary>
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
