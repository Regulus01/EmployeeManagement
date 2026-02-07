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
                   .WithMany()
                   .HasForeignKey(d => d.ParentDepartmentId);

            builder.HasOne<Employee>()
                   .WithMany()
                   .HasForeignKey(d => d.ManagerId);

            builder.Navigation(d => d.Employees)
                   .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
