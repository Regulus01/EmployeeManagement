namespace EmployeeManagement.Application.UseCases.Department.Create
{
    public sealed class CreateDepartmentResponse
    {
        /// <summary>
        /// Identificador único do departamento.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Nome do departamento.
        /// </summary>
        public string Nome { get; init; } = string.Empty;

        /// <summary>
        /// Identificador do gestor responsável pelo departamento (opcional).
        /// </summary>
        public Guid? ManagerId { get; init; }

        /// <summary>
        /// Identificador do departamento pai na hierarquia (opcional).
        /// </summary>
        public Guid? ParentDepartmentId { get; init; }
    }
}
