using FluentResults;
using MediatR;

namespace EmployeeManagement.Application.UseCases.Department.GetList
{
    /// <summary>
    /// Request para listagem de departamentos com filtros opcionais.
    /// </summary>
    public class GetListDepartmentRequest : IRequest<Result<GetListDepartmentResponse>>
    {
        /// <summary>
        /// Filtro pelo nome do departamento (contém).
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Filtro pelo nome do gerente (nome do colaborador responsável).
        /// </summary>
        public string? ManagerName { get; set; }

        /// <summary>
        /// Filtro pelo nome do departamento superior.
        /// </summary>
        public string? ParentDepartmentName { get; set; }
    }
}