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
                .WithMessage("Gerente inválido.");

            RuleFor(x => x.ParentDepartmentId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("Departamento inválido.")
                .MustAsync(IsDepartmentValid).WithMessage("Departamento não cadastrado.");
        }

        /// <summary>
        /// Verifica se já não existe um departamento com o mesmo nome.
        /// </summary>
        /// <param name="request">Requisição de criação do departamento.</param>
        /// <param name="nome">Nome do departamento a ser validado.</param>
        /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
        /// <returns>
        /// <c>true</c> se o nome for único; caso já exista um departamento com o mesmo nome, retorna <c>false</c>.
        /// </returns>
        private async Task<bool> BeUniqueName(CreateDepartmentRequest request, string nome, CancellationToken cancellationToken)
        {
            var existing = await _departmentRepository.GetByNameAsync(nome);
            return existing is null;
        }

        /// <summary>
        /// Verifica se o departamento informado existe no sistema.
        /// Caso o identificador seja nulo, a validação é considerada válida.
        /// </summary>
        /// <param name="id">Identificador do departamento (opcional).</param>
        /// <param name="cancellationToken">Token para cancelamento da operação assíncrona.</param>
        /// <returns>
        /// <c>true</c> se o identificador for nulo ou se o departamento existir; caso contrário, <c>false</c>.
        /// </returns>
        private async Task<bool> IsDepartmentValid(Guid? id, CancellationToken cancellationToken)
        {
            if (id == null)
                return true;

            var existing = await _departmentRepository.GetByIdAsync(id.Value);
            return existing != null;
        }
    }
}
