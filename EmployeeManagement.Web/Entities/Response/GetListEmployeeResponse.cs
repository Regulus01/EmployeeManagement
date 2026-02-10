namespace EmployeeManagement.Web.Entities.Response
{
    public class GetListEmployeeResponse
    {
        public List<GetListEmployeeDto> Employees { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class GetListEmployeeDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string? RG { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? ManagerName { get; set; }
    }
}
