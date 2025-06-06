using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Exceptions;
using ProjectManager.Domain.Interface;

namespace ProjectManager.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;

        public TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetTasksByProjectAsync(Guid projectId, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetTasksByProjectAsync(projectId, cancellationToken);

            if (!tasks.Any())
            {
                throw new NotFoundException("Nenhuma tarefa encontrada para o projeto.");
            }

            return tasks;
        }

        public async Task<Domain.Entities.Task> CreateTaskAsync(
            Guid projectId,
            CreateTaskDto data,
            CancellationToken cancellationToken)
        {
            await EnsureProjectExistsAsync(projectId, cancellationToken);
            await EnsureTaskLimitNotExceededAsync(projectId, cancellationToken);

            var task = new Domain.Entities.Task(data.Priority)
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                Title = data.Title,
                Description = data.Description,
                DueDate = data.DueDate,
                ResponsibleUserId = data.ResponsibleUserId,
                Status = Domain.Enums.TaskStatus.Pending
            };

            await _taskRepository.AddTaskAsync(task, cancellationToken);
            return task;
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(
            UpdateTaskDto updateTaskDto,
            CancellationToken cancellationToken
        )
        {
            var task = await GetTaskOrThrowAsync(updateTaskDto.ProjectId, updateTaskDto.TaskId, cancellationToken);

            if (updateTaskDto.NewStatus.HasValue)
                task.UpdateStatus(updateTaskDto.NewStatus.Value, updateTaskDto.ModifiedBy);

            if (!string.IsNullOrWhiteSpace(updateTaskDto.NewTitle) ||
                !string.IsNullOrWhiteSpace(updateTaskDto.NewDescription) ||
                updateTaskDto.NewDueDate.HasValue)
            {
                task.UpdateDetails(
                    updateTaskDto.NewTitle ?? task.Title,
                    updateTaskDto.NewDescription ?? task.Description,
                    updateTaskDto.NewDueDate ?? task.DueDate,
                    updateTaskDto.ModifiedBy);
            }

            await _taskRepository.UpdateTaskAsync(task, cancellationToken);
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(Guid projectId, Guid taskId, CancellationToken cancellationToken)
        {
            var task = await GetTaskOrThrowAsync(projectId, taskId, cancellationToken);
            await _taskRepository.DeleteTaskAsync(task, cancellationToken);
        }

        public async System.Threading.Tasks.Task AddCommentAsync(Guid projectId, Guid taskId, string comment, Guid user, CancellationToken cancellationToken)
        {
            var task = await GetTaskOrThrowAsync(projectId, taskId, cancellationToken);
            task.AddComment(comment, user);
            await _taskRepository.UpdateTaskAsync(task, cancellationToken);
        }

        private async Task<Domain.Entities.Task> GetTaskOrThrowAsync(Guid projectId, Guid taskId, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetTaskByIdAsync(projectId, taskId, cancellationToken);
            return task ?? throw new ArgumentException("Tarefa não encontrada.");
        }

        private async System.Threading.Tasks.Task EnsureProjectExistsAsync(Guid projectId, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(projectId, cancellationToken);
            if (project == null)
                throw new ArgumentException("Projeto não encontrado.");
        }

        private async System.Threading.Tasks.Task EnsureTaskLimitNotExceededAsync(Guid projectId, CancellationToken cancellationToken)
        {
            var existingTasks = await _taskRepository.GetTasksByProjectAsync(projectId, cancellationToken);
            if (existingTasks.Count() >= 20)
                throw new InvalidOperationException("Limite de 20 tarefas atingido para este projeto.");
        }
    }
}
