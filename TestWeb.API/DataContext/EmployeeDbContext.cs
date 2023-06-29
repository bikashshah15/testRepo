using Microsoft.EntityFrameworkCore;
using TestWeb.API.Models;

namespace TestWeb.API.DataContext
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options): base(options) { 

        }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Login> Login { get; set; }
    }
}
