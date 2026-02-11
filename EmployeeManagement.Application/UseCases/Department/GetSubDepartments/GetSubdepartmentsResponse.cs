namespace EmployeeManagement.Application.UseCases.Department.GetSubDepartments
{
    public class GetSubdepartmentsResponse
    {
        /// <summary>
        /// Nome do departamento atual.
        /// </summary>
        public string Nome { get; set; } = string.Empty;
        /// <summary>
        /// Departamento pai na hierarquia.
        /// </summary>
        public GetSubdepartmentsResponse? Parent { get; set; }

    }
}