using EmployeeManagement.Application.Common;
using MediatR;

namespace EmployeeManagement.Application.UseCases.Employee.Create
{
    public class CreateEmployeeRequest : IRequest<Result<CreateEmployeeResponse>>
    {
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
