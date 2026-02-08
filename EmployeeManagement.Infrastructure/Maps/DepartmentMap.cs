using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Infrastructure.Maps
{
    internal sealed class DepartmentMap : BaseEntityMap<Department>, IEntityTypeConfiguration<Department>
    {
        public override void Configure(EntityTypeBuilder<Department> builder)
        {
            base.Configure(builder);

            builder.ToTable("Departments");

            builder.Property(d => d.Nome)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(d => d.ParentDepartment)
                   .WithMany(d => d.SubDepartments)
                   .HasForeignKey(d => d.ParentDepartmentId);

            builder.Property(e => e.ManagerId)
                   .IsRequired(false);

            builder.HasOne(d => d.Manager)
                   .WithMany()
                   .HasForeignKey(d => d.ManagerId)
                   .IsRequired(false);

            builder.HasMany(d => d.Employees)
                   .WithOne(e => e.Department)
                   .HasForeignKey(e => e.DepartmentId);
        }
    }
}
