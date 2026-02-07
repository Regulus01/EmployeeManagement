using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Entities;

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