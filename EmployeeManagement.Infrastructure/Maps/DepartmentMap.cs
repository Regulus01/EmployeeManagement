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
                   .HasForeignKey(d => d.ParentDepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Manager)
                   .WithMany()
                   .HasForeignKey(d => d.ManagerId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(d => d.Employees)
                   .WithOne()
                   .HasForeignKey("DepartmentId")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(d => d.Employees)
                   .UsePropertyAccessMode(PropertyAccessMode.Property);

            builder.Navigation(d => d.SubDepartments)
                   .UsePropertyAccessMode(PropertyAccessMode.Property);
        }
    }
}
