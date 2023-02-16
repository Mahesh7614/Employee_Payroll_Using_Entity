using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Context
{
    public class EmployeePayrollContext : DbContext
    {
        public EmployeePayrollContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UserEntity> UserTable { get; set; }
    }
}
