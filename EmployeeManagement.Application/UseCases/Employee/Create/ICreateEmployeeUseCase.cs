using EmployeeManagement.Application.Common;

namespace EmployeeManagement.Application.UseCases.Employee.Create
{
    public interface ICreateEmployeeUseCase
    {
        Task<Result<CreateEmployeeResponse>> Handle(CreateEmployeeRequest request, CancellationToken cancellationToken = default);
    }
}