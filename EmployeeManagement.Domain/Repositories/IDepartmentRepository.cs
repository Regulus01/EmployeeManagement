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
    internal interface IDepartmentRepository
    {
        /// <summary>
        /// Anexa uma nova instância de <see cref="Department"/> ao contexto.
        /// A persistência no banco só ocorrerá após chamada a <see cref="SaveChangesAsync"/>.
        /// </summary>
        Task AddAsync(Department department);

        /// <summary>
        /// Persiste as alterações pendentes no repositório e retorna a entidade persistida.
        /// </summary>
        Task<Department> SaveChangesAsync(Department department);

        /// <summary>
        /// Recupera um departamento pelo identificador.
        /// </summary>
        Task<Department?> GetByIdAsync(Guid id);

        /// <summary>
        /// Recupera todos os departamentos.
        /// </summary>
        Task<IEnumerable<Department>> GetAllAsync();

        /// <summary>
        /// Recupera um departamento pelo nome.
        /// </summary>
        Task<Department?> GetByNameAsync(string nome);

        /// <summary>
        /// Atualiza um departamento existente.
        /// </summary>
        Task UpdateAsync(Department department);

        /// <summary>
        /// Remove um departamento pelo identificador.
        /// </summary>
        Task DeleteAsync(Guid id);
    }
}