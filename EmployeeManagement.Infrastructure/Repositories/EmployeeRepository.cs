using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Repositories;
using EmployeeManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace EmployeeManagement.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Employee> Get(
            Expression<Func<Employee, bool>>? filter = null, 
            int? skip = null, 
            int? take = null)
        {
            var query = _context.Employees.AsNoTracking();

            if (filter is not null)
                query = query.Where(filter);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return query.Include(x => x.Department).ToList();
        }

        public async Task AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            await _context.Employees.AddAsync(employee, cancellationToken);
        }

        public async Task<bool> SaveChangesAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            var saveChangesResult = await _context.SaveChangesAsync(cancellationToken);
            return saveChangesResult >= 1;
        }

        public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.AsNoTracking().ToListAsync();
        }

        public async Task<Employee?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
        {
            return await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.CPF == cpf, cancellationToken);
        }

        public async Task UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Employees.FindAsync(id, cancellationToken);

            if (entity is null)
                return;

            _context.Employees.Remove(entity);
        }
    }
}
