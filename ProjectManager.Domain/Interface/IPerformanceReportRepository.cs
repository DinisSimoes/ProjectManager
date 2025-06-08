using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Interface
{
    public interface IPerformanceReportRepository
    {
        Task<Dictionary<Guid, double>> GetAverageCompletedTasksPerUserLast30DaysAsync(CancellationToken cancellationToken);
    }
}
