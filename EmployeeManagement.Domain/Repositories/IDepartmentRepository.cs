using EmployeeManagement.Domain.Entities;
using System.Linq.Expressions;

namespace EmployeeManagement.Domain.Repositories
{
    /// <summary>
    /// Interface do repositório de <see cref="Department"/>.
    /// Segue o mesmo padrão da interface do Employee: anexar, persistir (SaveChangesAsync),
    /// operações de consulta, atualização e remoção.
    /// </summary>
    public interface IDepartmentRepository
    {
        /// <summary>
        /// Anexa uma nova instância de <see cref="Department"/> ao contexto.
        /// </summary>
        Task AddAsync(Department department, CancellationToken cancellationToken = default);

        /// <summary>
        /// Persiste as alterações pendentes no repositório e retorna a entidade persistida.
        /// </summary>
        Task<bool> SaveChangesAsync(Department department, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtém departamentos com suporte a filtro e paginação opcionais.
        /// </summary>
        /// <param name="filter">
        /// Expressão para filtrar os departamentos.
        /// Quando não informada, retorna todos os registros.
        /// </param>
        /// <param name="skip">
        /// Quantidade de registros a ignorar na consulta (paginação).
        /// </param>
        /// <param name="take">
        /// Quantidade máxima de registros a retornar.
        /// </param>
        /// <returns>
        /// Coleção de <see cref="Department"/> resultante da consulta.
        /// </returns>
        IEnumerable<Department> Get(
            Expression<Func<Department, bool>>? filter = null,
            int? skip = null,
            int? take = null);

        /// <summary>
        /// Recupera um departamento pelo identificador.
        /// </summary>
        Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Recupera todos os departamentos.
        /// </summary>
        Task<IEnumerable<Department>> GetAllAsync();

        /// <summary>
        /// Recupera um departamento pelo nome.
        /// </summary>
        Task<Department?> GetByNameAsync(string nome, CancellationToken cancellationToken = default);

        /// <summary>
        /// Atualiza um departamento existente.
        /// </summary>
        Task UpdateAsync(Department department, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove um departamento pelo identificador.
        /// </summary>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}