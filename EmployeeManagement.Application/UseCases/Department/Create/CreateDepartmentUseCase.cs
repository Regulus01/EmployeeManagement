using EmployeeManagement.Domain.Repositories;

namespace EmployeeManagement.Application.UseCases.Department.Create
{
    internal sealed class CreateDepartmentUseCase
    {
        private readonly IDepartmentRepository _departmentRepository;
        public CreateDepartmentUseCase(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        }

        public async Task<CreateDepartmentResponse> Execute(CreateDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            var department = new Domain.Entities.Department(
                request.Nome,
                request.ManagerId,
                request.ParentDepartmentId);

            await _departmentRepository.AddAsync(department);
            var persisted = await _departmentRepository.SaveChangesAsync(department);

            return new CreateDepartmentResponse
            {
                Id = persisted.Id,
                Nome = persisted.Nome,
                ManagerId = persisted.ManagerId,
                ParentDepartmentId = persisted.ParentDepartmentId
            };
        }
    }
}
