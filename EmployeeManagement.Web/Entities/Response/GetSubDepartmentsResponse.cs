namespace EmployeeManagement.Web.Entities.Response
{
    public class GetSubDepartmentsResponse
    {
        public string Nome { get; set; } = string.Empty;
        public GetSubDepartmentsResponse? Parent { get; set; }
    }
}