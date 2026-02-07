using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Repositories;
using EmployeeManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    internal class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
        }

        public async Task<Department> SaveChangesAsync(Department department)
        {
            await _context.SaveChangesAsync();
            return department;
        }

        public async Task<Department?> GetByIdAsync(Guid id)
        {
            return await _context.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments.AsNoTracking().ToListAsync();
        }

        public async Task<Department?> GetByNameAsync(string nome)
        {
            return await _context.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Nome == nome);
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Departments.FindAsync(id);

            if (entity is null) 
                return;

            _context.Departments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}