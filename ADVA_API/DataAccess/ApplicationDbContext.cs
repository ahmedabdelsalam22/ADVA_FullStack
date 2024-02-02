using ADVA_API.Models;
using Microsoft.EntityFrameworkCore;

namespace ADVA_API.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Employee>()
            //    .HasOne(e => e.Department)
            //    .WithMany(d => d.Employees)
            //    .HasForeignKey(e => e.DepartmentID);

            //modelBuilder.Entity<Employee>()
            //    .HasOne(e => e.Manager)
            //    .WithMany()
            //    .HasForeignKey(e => e.ManagerID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
