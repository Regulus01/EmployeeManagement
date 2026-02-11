using EmployeeManagement.Web.Clients;
using EmployeeManagement.Web.Entities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagement.Web.Pages.Departments
{
    public class SubDepartments : PageModel
    {
        private readonly IDepartmentClient _client;

        public SubDepartments(IDepartmentClient client)
        {
            _client = client;
        }

        [BindProperty(SupportsGet = true)]
        public Guid DepartmentId { get; set; }

        public IEnumerable<GetSubDepartmentsResponse>? Root { get; set; }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            if (DepartmentId == Guid.Empty)
            {
                Root = null;
                return;
            }

            var response = await _client.GetSubDepartmentsAsync(DepartmentId, cancellationToken);

            Root = response;
        }
    }
}