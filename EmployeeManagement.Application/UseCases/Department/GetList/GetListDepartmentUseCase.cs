using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.Extensions;
using EmployeeManagement.Domain.Repositories;
using MediatR;
using System.Linq.Expressions;

namespace EmployeeManagement.Application.UseCases.Department.GetList
{
    /// <summary>
    /// Use case para listagem de departamentos com filtros opcionais.
    /// </summary>
    public class GetListDepartmentUseCase : IRequestHandler<GetListDepartmentRequest, Result<GetListDepartmentResponse>>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public GetListDepartmentUseCase(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        }

        public async Task<Result<GetListDepartmentResponse>> Handle(GetListDepartmentRequest request, CancellationToken cancellationToken)
        {

            Expression<Func<Domain.Entities.Department, bool>> filter = x => true;

            if (!string.IsNullOrWhiteSpace(request.Nome))
                filter = filter.And(x => x.Nome.ToLower().Contains(request.Nome.ToLower()));

            if (!string.IsNullOrWhiteSpace(request.ManagerName))
                filter = filter.And(x => x.Manager.Nome.ToLower().Contains(request.ManagerName.ToLower()));

            if (!string.IsNullOrWhiteSpace(request.ParentDepartmentName))
                filter = filter.And(x => x.ParentDepartment.Nome.ToLower().Contains(request.ParentDepartmentName.ToLower()));


            var departments = _departmentRepository.Get(filter);

            var getListDepartmentDto =
                departments.Select(d => new GetListDepartmentDto
                {
                    Id = d.Id,
                    Nome = d.Nome,
                    ManagerName = d.Manager?.Nome,
                    ParentDepartmentName = d.ParentDepartment?.Nome
                })
                .ToList();

            var response = new GetListDepartmentResponse
            {
                Departments = getListDepartmentDto,
                TotalCount = departments.Count()
            };

            return Result.Success(response);
        }
    }
}