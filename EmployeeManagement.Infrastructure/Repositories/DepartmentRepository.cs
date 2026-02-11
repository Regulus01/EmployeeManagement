using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Repositories;
using EmployeeManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeeManagement.Infrastructure.Repositories
{
    internal class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Department department, CancellationToken cancellationToken = default)
        {
            await _context.Departments.AddAsync(department, cancellationToken);
        }

        public async Task<bool> SaveChangesAsync(Department department, CancellationToken cancellationToken = default)
        {
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result >= 1;
        }

        public IEnumerable<Department> Get(
            Expression<Func<Department, bool>>? filter = null,
            int? skip = null,
            int? take = null)
        {
            var query = _context.Departments.AsNoTracking();

            if (filter is not null)
                query = query.Where(filter);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return query.Include(x => x.Manager)
                        .Include(x => x.ParentDepartment)
                        .ToList();
        }

        public async Task<Department?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .Include(d => d.ParentDepartment)
                .ThenInclude(d => d.ParentDepartment)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments.AsNoTracking().ToListAsync();
        }

        public async Task<Department?> GetByNameAsync(string nome, CancellationToken cancellationToken = default)
        {
            return await _context.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Nome == nome, cancellationToken);
        }

        public async Task UpdateAsync(Department department, CancellationToken cancellationToken = default)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Departments.FindAsync(id, cancellationToken);

            if (entity is null)
                return;

            _context.Departments.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}