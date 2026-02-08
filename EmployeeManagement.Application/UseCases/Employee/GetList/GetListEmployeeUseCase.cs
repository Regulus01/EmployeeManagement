using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.Extensions;
using EmployeeManagement.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;

namespace EmployeeManagement.Application.UseCases.Employee.GetList
{
    /// <summary>
    /// Use case para listagem de colaboradores com filtros opcionais.
    /// </summary>
    public class GetListEmployeeUseCase : IRequestHandler<GetListEmployeeRequest, Result<GetListEmployeeResponse>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="GetListEmployeeUseCase"/>.
        /// </summary>
        /// <param name="employeeRepository">Repositório de colaboradores.</param>
        /// <param name="departmentRepository">Repositório de departamentos.</param>
        public GetListEmployeeUseCase(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        }

        /// <summary>
        /// Processa a requisição de listagem de colaboradores.
        /// </summary>
        /// <param name="request">Request com filtros opcionais.</param>
        /// <param name="cancellationToken">Token de cancelamento.</param>
        /// <returns>Result contendo a lista de colaboradores ou erros de validação.</returns>
        public async Task<Result<GetListEmployeeResponse>> Handle(
            GetListEmployeeRequest request,
            CancellationToken cancellationToken)
        {
            Expression<Func<Domain.Entities.Employee, bool>> filter = x => true;

            if (!string.IsNullOrWhiteSpace(request.Nome))
                filter = filter.And(x => x.Nome.ToLower().Contains(request.Nome.ToLower()));

            if (!string.IsNullOrWhiteSpace(request.CPF))
                filter = filter.And(x => x.CPF.Equals(request.CPF));

            if (!string.IsNullOrWhiteSpace(request.RG))
                filter = filter.And(x => x.RG != null && x.RG.ToLower().Contains(request.RG.ToLower()));

            if (request.DepartmentId.HasValue)
                filter = filter.And(x => x.DepartmentId == request.DepartmentId.Value);     
    
            var employees = _employeeRepository.Get(filter);

            var employeeDtos = new List<EmployeeDto>();
    
            foreach (var employee in employees)
            {
                employeeDtos.Add(new EmployeeDto
                {
                    Nome = employee.Nome,
                    CPF = employee.CPF,
                    RG = employee.RG,
                    DepartmentName = employee.Department.Nome
                });
            }

            var response = new GetListEmployeeResponse
            {
                Employees = employeeDtos,
                TotalCount = employeeDtos.Count
            };

            return Result<GetListEmployeeResponse>.Success(response);
        }
    }
}
