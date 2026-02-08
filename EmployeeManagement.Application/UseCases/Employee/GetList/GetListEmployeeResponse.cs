using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.UseCases.Employee.GetList
{
    /// <summary>
    /// Response da listagem de colaboradores.
    /// </summary>
    public class GetListEmployeeResponse
    {
        /// <summary>
        /// Lista de colaboradores encontrados.
        /// </summary>
        public List<EmployeeDto> Employees { get; set; } = new();

        /// <summary>
        /// Total de registros encontrados.
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// DTO representando um colaborador na listagem.
    /// </summary>
    public class EmployeeDto
    {

        /// <summary>
        /// Nome do colaborador.
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// CPF do colaborador.
        /// </summary>
        public string CPF { get; set; } = string.Empty;

        /// <summary>
        /// RG do colaborador (opcional).
        /// </summary>
        public string? RG { get; set; }

        /// <summary>
        /// Nome do departamento ao qual o colaborador pertence.
        /// </summary>
        public string DepartmentName { get; set; } = string.Empty;
    }
}
