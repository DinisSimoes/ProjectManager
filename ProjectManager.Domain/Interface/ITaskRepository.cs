using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Interface
{
    public interface ITaskRepository
    {
        Task<IEnumerable<Entities.Task>> GetTasksByProjectAsync(Guid projectId, CancellationToken cancellationToken);

        Task<Entities.Task?> GetTaskByIdAsync(Guid projectId, Guid taskId, CancellationToken cancellationToken);

        Task AddTaskAsync(Entities.Task task, CancellationToken cancellationToken);

        Task UpdateTaskAsync(Entities.Task task, CancellationToken cancellationToken);

        Task DeleteTaskAsync(Entities.Task task, CancellationToken cancellationToken);

        Task<int> CountTasksByProjectAsync(Guid projectId, CancellationToken cancellationToken);
    }
}
