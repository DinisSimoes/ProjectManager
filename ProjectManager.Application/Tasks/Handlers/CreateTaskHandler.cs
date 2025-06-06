using MediatR;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.Services;
using ProjectManager.Application.Tasks.Commands;

namespace ProjectManager.Application.Tasks.Handlers
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, Domain.Entities.Task>
    {
        private readonly ITaskService _taskService;

        public CreateTaskHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<Domain.Entities.Task> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var taskData = new CreateTaskDto
            {
                Title = request.Title,
                Description = request.Description,
                DueDate = request.DueDate,
                Priority = request.Priority,
                ResponsibleUserId = request.ResponsibleUserId
            };

            return await _taskService.CreateTaskAsync(
                request.ProjectId,
                taskData,
                cancellationToken);
        }
    }
}
