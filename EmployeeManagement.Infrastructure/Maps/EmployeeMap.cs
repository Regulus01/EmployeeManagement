using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Infrastructure.Maps
{
    internal sealed class EmployeeMap : BaseEntityMap<Employee>, IEntityTypeConfiguration<Employee>
    {
        public override void Configure(EntityTypeBuilder<Employee> builder)
        {
            base.Configure(builder);

            builder.ToTable("Employees");

            builder.Property(e => e.Nome)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.CPF)
                   .IsRequired()
                   .HasMaxLength(11);

            builder.HasIndex(e => e.CPF)
                   .IsUnique();

            builder.Property(e => e.RG)
                   .HasMaxLength(20);

            builder.Property(e => e.DepartmentId)
                   .IsRequired();

            builder.HasOne<Department>()
                   .WithMany(d => d.Employees)
                   .HasForeignKey(e => e.DepartmentId);
        }
    }
}
