using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
