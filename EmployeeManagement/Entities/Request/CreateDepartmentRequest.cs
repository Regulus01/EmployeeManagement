namespace EmployeeManagement.Web.Entities.Request
{
    public class CreateDepartmentRequest
    {
        public string Nome { get; init; } = string.Empty;
        public Guid? ManagerId { get; init; }
        public Guid? ParentDepartmentId { get; init; }
    }
}
