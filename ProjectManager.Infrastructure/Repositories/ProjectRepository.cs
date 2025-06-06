using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interface;
using ProjectManager.Infrastructure.Data;

namespace ProjectManager.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Projects
                .Where(p => p.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async System.Threading.Tasks.Task AddAsync(Project project, CancellationToken cancellationToken)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Projects.FindAsync(new object[] { id }, cancellationToken);
        }
    }
}
