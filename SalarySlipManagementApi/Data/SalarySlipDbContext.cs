using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SalarySlipManagementApi.Entities;

namespace SalarySlipManagementApi.Data
{
    public class SalarySlipDbContext : DbContext
    {
        public SalarySlipDbContext(DbContextOptions<SalarySlipDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<SalaryStructure> SalaryStructures { get; set; }
        public DbSet<MonthlySalarySlip> MonthlySalarySlips { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Configure standard Employee rules &relationships

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity
                    .HasOne(e => e.Role)
                    .WithMany(r => r.Employees)
                    .HasForeignKey(e => e.RoleId)
                    .HasConstraintName("FK_Employee_Role")
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentId)
                    .HasConstraintName("FK_Employee_Department")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder
                .Entity<SalaryStructure>()
                .HasOne(s => s.Employee)
                .WithOne(e => e.SalaryStructure)
                .HasForeignKey<SalaryStructure>(s => s.EmployeeId)
                .HasConstraintName("FK_SalaryStructure_Employee")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<MonthlySalarySlip>()
                .HasOne(m => m.Employee)
                .WithMany(e => e.MonthlySalarySlips)
                .HasForeignKey(m => m.EmployeeId)
                .HasConstraintName("FK_MonthlySalarySlip_Employee")
                .OnDelete(DeleteBehavior.Restrict);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType
                    .ClrType.GetProperties()
                    .Where(p =>
                        p.PropertyType == typeof(decimal) || p.PropertyType == typeof(decimal?)
                    );

                foreach (var property in properties)
                {
                    modelBuilder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasColumnType("decimal(18, 2)");
                }

                var isActiveProperty = entityType.FindProperty("IsActive");
                if (isActiveProperty != null && isActiveProperty.ClrType == typeof(bool))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var propertyAccess = Expression.Property(parameter, "IsActive");
                    var filterExpression = Expression.Lambda(propertyAccess, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filterExpression);
                }
            }
        }
    }
}
