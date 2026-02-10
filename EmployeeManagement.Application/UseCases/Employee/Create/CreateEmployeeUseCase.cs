using EmployeeManagement.Application.Common;
using EmployeeManagement.Domain.Repositories;
using MediatR;

namespace EmployeeManagement.Application.UseCases.Employee.Create
{
    public class CreateEmployeeUseCase : IRequestHandler<CreateEmployeeRequest, Result<CreateEmployeeResponse>>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public CreateEmployeeUseCase(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public async Task<Result<CreateEmployeeResponse>> Handle(CreateEmployeeRequest request, CancellationToken cancellationToken = default)
        {
            var employee = new Domain.Entities.Employee(
                request.Nome,
                request.CPF,
                request.RG,
                request.DepartmentId);

            await _employeeRepository.AddAsync(employee, cancellationToken);

            var saveChangesSuccess = await _employeeRepository.SaveChangesAsync(employee, cancellationToken);

            if (!saveChangesSuccess)
                return Result.Failure<CreateEmployeeResponse>(["Falha ao salvar o funcionário."]);

            var response = new CreateEmployeeResponse
            {
                Id = employee.Id,
                Nome = employee.Nome,
                CPF = employee.CPF,
                RG = employee.RG,
                DepartmentId = employee.DepartmentId
            };

            return Result.Success(response);
        }
    }

}
