using EmployeeManagement.Application.Common;
using MediatR;

namespace EmployeeManagement.Application.UseCases.Employee.GetList
{
    /// <summary>
    /// Request para listagem de colaboradores com filtros opcionais.
    /// </summary>
    public class GetListEmployeeRequest : IRequest<Result<GetListEmployeeResponse>>
    {
        /// <summary>
        /// Filtro por nome do colaborador.
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Filtro por CPF do colaborador.
        /// </summary>
        public string? CPF { get; set; }

        /// <summary>
        /// Filtro por RG do colaborador.
        /// </summary>
        public string? RG { get; set; }

        /// <summary>
        /// Filtro por ID do departamento associado ao colaborador.
        /// </summary>
        public Guid? DepartmentId { get; set; }
    }
}
