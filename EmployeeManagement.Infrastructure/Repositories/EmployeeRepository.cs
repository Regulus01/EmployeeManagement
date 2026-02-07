using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Repositories;
using EmployeeManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Repositories
{
    internal class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }

        public async Task<Employee> SaveChangesAsync(Employee employee)
        {
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.AsNoTracking().ToListAsync();
        }

        public async Task<Employee?> GetByCpfAsync(string cpf)
        {
            return await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.CPF == cpf);
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Employees.FindAsync(id);

            if (entity is null) 
                return;

            _context.Employees.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
