using EmployeeManagement.Domain.Repositories;
using FluentValidation;

namespace EmployeeManagement.Application.UseCases.Department.Create
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentRequest>
    {
        private readonly IDepartmentRepository _departmentRepository;

        public CreateDepartmentValidator(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MinimumLength(2).WithMessage("Nome deve ter ao menos 2 caracteres.")
                .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.")
                .MustAsync(BeUniqueName).WithMessage("Já existe um departamento com esse nome.");

            RuleFor(x => x.ManagerId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("ManagerId inválido.");

            RuleFor(x => x.ParentDepartmentId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("ParentDepartmentId inválido.");
        }

        private async Task<bool> BeUniqueName(CreateDepartmentRequest request, string nome, CancellationToken cancellationToken)
        {
            var existing = await _departmentRepository.GetByNameAsync(nome);
            return existing is null;
        }
    }
}
