using EmployeeManagement.Domain.Repositories;
using FluentValidation;
using System.Text.RegularExpressions;

namespace EmployeeManagement.Application.UseCases.Employee.Create
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeRequest>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public CreateEmployeeValidator(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MinimumLength(2).WithMessage("Nome deve ter ao menos 2 caracteres.")
                .MaximumLength(50).WithMessage("Nome deve ter no máximo 50 caracteres.");

            RuleFor(x => x.CPF)
                .NotEmpty().WithMessage("CPF é obrigatório.")
                .Must(IsValidFormat).WithMessage("CPF inválido.")
                .MustAsync(IsUniqueCpf).WithMessage("CPF já cadastrado.");

            RuleFor(x => x.DepartmentId)
                .Must(id => id != Guid.Empty).WithMessage("DepartmentId é obrigatório.")
                .MustAsync(IsDepartmentValid).WithMessage("Departamento não cadastrado.");
     
        }

        private async Task<bool> IsUniqueCpf(string cpf, CancellationToken cancellationToken)
        {
            var existing = await _employeeRepository.GetByCpfAsync(cpf);
            return existing is null;
        }

        private async Task<bool> IsDepartmentValid(Guid id, CancellationToken cancellationToken)
        {
            var existing = await _departmentRepository.GetByIdAsync(id);
            return existing != null;
        }

        private static bool IsValidFormat(string cpf)
        {
            return Regex.IsMatch(cpf, @"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$");
        }
    }
}
