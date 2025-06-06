using ProjectManager.Application.DTOs;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<Domain.Entities.Task>> GetTasksByProjectAsync(Guid projectId, CancellationToken cancellationToken);
        Task<Domain.Entities.Task> CreateTaskAsync(Guid projectId, CreateTaskDto data, CancellationToken cancellationToken);
        System.Threading.Tasks.Task UpdateTaskAsync(UpdateTaskDto updateTaskDto, CancellationToken cancellationToken);
        System.Threading.Tasks.Task DeleteTaskAsync(Guid projectId, Guid taskId, CancellationToken cancellationToken);
        System.Threading.Tasks.Task AddCommentAsync(Guid projectId, Guid taskId, string comment, Guid user, CancellationToken cancellationToken);
    }
}
