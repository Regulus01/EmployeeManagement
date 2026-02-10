using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Maps;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Context
{
    public sealed class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmployeeMap());
            modelBuilder.ApplyConfiguration(new DepartmentMap());
        }
    }
}
