using API_Endpoints_Salary_Calculator.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Endpoints_Salary_Calculator.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<SalaryCalculation> SalaryCalculations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.HourlyRate).HasPrecision(18, 2);
            });

            modelBuilder.Entity<SalaryCalculation>(entity =>
            {
                entity.HasKey(sc => sc.Id);
                entity.HasOne(sc => sc.Employee)
                    .WithMany(e => e.SalaryCalculations)
                    .HasForeignKey(sc => sc.EmployeeId);

                entity.Property(sc => sc.HourlyRate).HasPrecision(18, 2);
                entity.Property(sc => sc.VariableAmount).HasPrecision(18, 2);
                entity.Property(sc => sc.BasicSalary).HasPrecision(18, 2);
                entity.Property(sc => sc.OvertimePay).HasPrecision(18, 2);
                entity.Property(sc => sc.GrossSalary).HasPrecision(18, 2);
                entity.Property(sc => sc.UIF).HasPrecision(18, 2);
                entity.Property(sc => sc.PAYE).HasPrecision(18, 2);
                entity.Property(sc => sc.RetirementFund).HasPrecision(18, 2);
                entity.Property(sc => sc.TotalDeductions).HasPrecision(18, 2);
                entity.Property(sc => sc.NetSalary).HasPrecision(18, 2);
                entity.Property(sc => sc.UIF_Employer).HasPrecision(18, 2);
                entity.Property(sc => sc.SDL).HasPrecision(18, 2);
                entity.Property(sc => sc.TotalEmployerCosts).HasPrecision(18, 2);
                entity.Property(sc => sc.CostToCompany).HasPrecision(18, 2);
            });
        }
    }
}
