using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Repositories;
using EmployeeManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading;

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
