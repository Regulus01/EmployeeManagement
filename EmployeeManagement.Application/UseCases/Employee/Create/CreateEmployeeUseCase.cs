using EmployeeManagement.Domain.Repositories;
using MediatR;

namespace EmployeeManagement.Application.UseCases.Employee.Create
{
    public class CreateEmployeeUseCase : IRequestHandler<CreateEmployeeRequest, CreateEmployeeResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public CreateEmployeeUseCase(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public async Task<CreateEmployeeResponse> Handle(CreateEmployeeRequest request, CancellationToken cancellationToken = default)
        {
            var employee = new Domain.Entities.Employee(
                request.Nome,
                request.CPF,
                request.RG,
                request.DepartmentId);

            await _employeeRepository.AddAsync(employee, cancellationToken);

            var saveChangesSuccess = await _employeeRepository.SaveChangesAsync(employee, cancellationToken);

            if (!saveChangesSuccess)
                throw new Exception("Failed to save employee.");

            return new CreateEmployeeResponse
            {
                Id = employee.Id,
                Nome = employee.Nome,
                CPF = employee.CPF,
                RG = employee.RG,
                DepartmentId = employee.DepartmentId
            };
        }
    }

}
