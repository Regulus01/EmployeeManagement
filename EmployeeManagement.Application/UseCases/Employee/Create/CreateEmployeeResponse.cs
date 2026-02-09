namespace EmployeeManagement.Application.UseCases.Employee.Create
{
    /// <summary>
    /// Resposta retornada após a criação de um colaborador.
    /// Contém os principais dados do colaborador recém-criado.
    /// </summary>
    public sealed class CreateEmployeeResponse
    {
        /// <summary>
        /// Identificador único do colaborador.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Nome completo do colaborador.
        /// </summary>
        public string Nome { get; init; } = string.Empty;

        /// <summary>
        /// CPF do colaborador.
        /// </summary>
        public string CPF { get; init; } = string.Empty;

        /// <summary>
        /// RG do colaborador (opcional).
        /// </summary>
        public string? RG { get; init; }

        /// <summary>
        /// Identificador do departamento ao qual o colaborador pertence.
        /// </summary>
        public Guid DepartmentId { get; init; }
    }
}
