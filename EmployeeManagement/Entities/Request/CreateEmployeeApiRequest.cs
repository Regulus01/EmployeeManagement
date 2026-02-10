namespace EmployeeManagement.Web.Entities.Request
{
    public class CreateEmployeeApiRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string? RG { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
