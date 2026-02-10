namespace EmployeeManagement.Web.Entities.Response
{
    public class GetListDepartmentResponse
    {
        public List<GetListDepartmentDto> Departments { get; set; } = new();
        public int TotalCount { get; set; }
    }
    public class GetListDepartmentDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? ManagerName { get; set; }
        public string? ParentDepartmentName { get; set; }
    }
}
