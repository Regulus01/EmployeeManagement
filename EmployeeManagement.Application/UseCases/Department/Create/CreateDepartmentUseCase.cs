using EmployeeManagement.Application.Common;
using EmployeeManagement.Domain.Repositories;
using MediatR;

namespace EmployeeManagement.Application.UseCases.Department.Create
{
    /// <summary>
    /// Use case para criação de departamento.
    /// </summary>
    public class CreateDepartmentUseCase : IRequestHandler<CreateDepartmentRequest, Result<CreateDepartmentResponse>>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public CreateDepartmentUseCase(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        }

        public async Task<Result<CreateDepartmentResponse>> Handle(CreateDepartmentRequest request, CancellationToken cancellationToken)
        {
            var department = new Domain.Entities.Department(request.Nome, request.ManagerId, request.ParentDepartmentId);

            await _departmentRepository.AddAsync(department, cancellationToken);

            var saveChangesSuccess = await _departmentRepository.SaveChangesAsync(department, cancellationToken);

            if (!saveChangesSuccess)
                return Result.Failure<CreateDepartmentResponse>(["Falha ao salvar o departamento."]);

            var response = new CreateDepartmentResponse
            {
                Id = department.Id,
                Nome = department.Nome
            };

            return Result.Success(response);
        }
    }
}
