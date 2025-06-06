using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Interface;
using ProjectManager.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByProjectAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _context.Tasks
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.History)
                .Include(t => t.Comments)
                .ToListAsync(cancellationToken);
        }

        public async Task<Domain.Entities.Task?> GetTaskByIdAsync(Guid projectId, Guid taskId, CancellationToken cancellationToken)
        {
            return await _context.Tasks
                .Include(t => t.History)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.ProjectId == projectId && t.Id == taskId, cancellationToken);
        }

        public async Task AddTaskAsync(Domain.Entities.Task task, CancellationToken cancellationToken)
        {
            await _context.Tasks.AddAsync(task, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateTaskAsync(Domain.Entities.Task task, CancellationToken cancellationToken)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteTaskAsync(Domain.Entities.Task task, CancellationToken cancellationToken)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> CountTasksByProjectAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _context.Tasks
                .CountAsync(t => t.ProjectId == projectId, cancellationToken);
        }
    }
}
