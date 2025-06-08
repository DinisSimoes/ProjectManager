using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }

        public DbSet<Domain.Entities.Task> Tasks { get; set; }

        public DbSet<TaskHistory> TasksHistory { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
