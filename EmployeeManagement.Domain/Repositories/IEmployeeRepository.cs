using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Domain.Repositories
{
    /// <summary>
    /// Interface do repositório de <see cref="Employee"/>.
    /// Define operações assíncronas de CRUD.
    /// </summary>
    internal interface IEmployeeRepository
    {
        /// <summary>
        /// Adiciona uma nova instância de <see cref="Employee"/> ao repositório (DbContext).
        /// </summary>
        /// <param name="employee">Instância de <see cref="Employee"/> a ser adicionada.</param>
        /// <param name="cancellationToken">Token para cancelar a operação assíncrona.</param>
        /// <returns>Task que representa a operação assíncrona de anexação ao contexto.</returns>
        Task AddAsync(Employee employee, CancellationToken cancellationToken = default);

        /// <summary>
        /// Persiste as alterações pendentes no repositório e retorna a entidade informada.
        /// </summary>
        /// <param name="employee">Entidade cuja persistência está sendo confirmada.</param>
        /// <param name="cancellationToken">Token para cancelar a operação assíncrona.</param>
        /// <returns>Booleano indicando se a operação foi bem sucedida.</returns>
        Task<bool> SaveChangesAsync(Employee employee, CancellationToken cancellationToken = default);

        /// <summary>
        /// Recupera um funcionário pelo identificador.
        /// </summary>
        /// <param name="id">Identificador (GUID) do funcionário.</param>
        /// <param name="cancellationToken">Token para cancelar a operação assíncrona.</param>
        /// <returns>Task com a entidade encontrada ou <c>null</c> se não existir.</returns>
        Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Recupera todos os funcionários do repositório.
        /// </summary>
        /// <returns>Task com a coleção de funcionários.</returns>
        Task<IEnumerable<Employee>> GetAllAsync();

        /// <summary>
        /// Recupera um funcionário pelo CPF.
        /// </summary>
        /// <param name="cpf">CPF do funcionário (formato esperado conforme validação da entidade).</param>
        /// <param name="cancellationToken">Token para cancelar a operação assíncrona.</param>
        /// <returns>Task com a entidade encontrada ou <c>null</c> se não existir.</returns>
        Task<Employee?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default);

        /// <summary>
        /// Atualiza um funcionário existente.
        /// </summary>
        /// <param name="employee">Entidade <see cref="Employee"/> com os dados atualizados.</param>
        /// <param name="cancellationToken">Token para cancelar a operação assíncrona.</param>
        /// <returns>Task representando a operação assíncrona.</returns>
        Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove um funcionário pelo identificador.
        /// </summary>
        /// <param name="id">Identificador (GUID) do funcionário a ser removido.</param>
        /// <param name="cancellationToken">Token para cancelar a operação assíncrona.</param>
        /// <returns>Task representando a operação assíncrona.</returns>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}