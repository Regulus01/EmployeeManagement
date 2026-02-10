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

        /// <summary>
        /// Verifica se o CPF informado ainda não está cadastrado no sistema.
        /// </summary>
        /// <param name="cpf">CPF a ser verificado.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
        /// <returns>
        /// <c>true</c> se o CPF for único; caso já exista um colaborador com o mesmo CPF, retorna <c>false</c>.
        /// </returns>
        private async Task<bool> IsUniqueCpf(string cpf, CancellationToken cancellationToken)
        {
            var existing = await _employeeRepository.GetByCpfAsync(cpf);
            return existing is null;
        }

        /// <summary>
        /// Verifica se o departamento informado existe no sistema.
        /// </summary>
        /// <param name="id">Identificador do departamento.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
        /// <returns>
        /// <c>true</c> se o departamento existir; caso contrário, <c>false</c>.
        /// </returns>
        private async Task<bool> IsDepartmentValid(Guid id, CancellationToken cancellationToken)
        {
            var existing = await _departmentRepository.GetByIdAsync(id);
            return existing != null;
        }

        /// <summary>
        /// Valida se o CPF possui um formato válido.
        /// Aceita CPF com ou sem pontuação (ex: 000.000.000-00 ou 00000000000).
        /// </summary>
        /// <param name="cpf">CPF a ser validado.</param>
        /// <returns>
        /// <c>true</c> se o formato do CPF for válido; caso contrário, <c>false</c>.
        /// </returns>
        private static bool IsValidFormat(string cpf)
        {
            return Regex.IsMatch(cpf, @"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$");
        }
    }
}
