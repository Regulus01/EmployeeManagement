using EmployeeManagement.Application.UseCases.Employee.Create;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/Employee")]
    internal class EmployeeController : ControllerBase
    {
        private readonly CreateEmployeeUseCase _createEmployeeUseCase;

        public EmployeeController(CreateEmployeeUseCase createEmployeeUseCase)
        {
            _createEmployeeUseCase = createEmployeeUseCase ?? throw new ArgumentNullException(nameof(createEmployeeUseCase));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken)
        {
            var response = await _createEmployeeUseCase.Execute(request, cancellationToken);

            return Created(string.Empty, response);
        }
    }
}
