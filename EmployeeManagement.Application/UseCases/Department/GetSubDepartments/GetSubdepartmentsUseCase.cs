using EmployeeManagement.Domain.Repositories;
using FluentResults;
using MediatR;

namespace EmployeeManagement.Application.UseCases.Department.GetSubDepartments
{
    public class GetSubdepartmentsUseCase : IRequestHandler<GetSubdepartmentsRequest, Result<List<GetSubdepartmentsResponse>>>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public GetSubdepartmentsUseCase(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<Result<List<GetSubdepartmentsResponse>>> Handle(GetSubdepartmentsRequest request, CancellationToken cancellationToken)
        {
            var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);

            if (department is null)
                return Result.Fail("Departamento não encontrado.");

            List<GetSubdepartmentsResponse> result = [MapRecursive(department)];

            return Result.Ok(result);
        }

        private GetSubdepartmentsResponse MapRecursive(Domain.Entities.Department department)
        {
            if (department.ParentDepartment is null)
            {
                return new GetSubdepartmentsResponse
                {
                    Nome = department.Nome
                };
            }

            return new GetSubdepartmentsResponse
            {
                Nome = department.Nome,
                Parent = MapRecursive(department.ParentDepartment)
            };
        }
    }
}