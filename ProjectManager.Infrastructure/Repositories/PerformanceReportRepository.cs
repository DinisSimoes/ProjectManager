using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Interface;
using ProjectManager.Infrastructure.Data;

namespace ProjectManager.Infrastructure.Repositories
{
    public class PerformanceReportRepository : IPerformanceReportRepository
    {
        private readonly ApplicationDbContext _context;

        public PerformanceReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<Guid, double>> GetAverageCompletedTasksPerUserLast30DaysAsync(CancellationToken cancellationToken)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-30);

            var completedTasks = await _context.Tasks
                .Where(t => t.Status == Domain.Enums.TaskStatus.Completed && t.History.Any(h => h.ModificationDate >= cutoffDate))
                .GroupBy(t => t.ResponsibleUserId)
                .Select(group => new
                {
                    UserId = group.Key,
                    CompletedCount = group.Count()
                })
                .ToListAsync(cancellationToken);

            return completedTasks.ToDictionary(x => x.UserId, x => x.CompletedCount / 30.0);
        }
    }
}
