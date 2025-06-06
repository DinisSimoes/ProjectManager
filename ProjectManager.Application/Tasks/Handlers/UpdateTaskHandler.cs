using MediatR;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.Tasks.Commands;

namespace ProjectManager.Application.Tasks.Handlers
{
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, Unit>
    {
        private readonly ITaskService _taskService;

        public UpdateTaskHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var updateTaskDto = new UpdateTaskDto
            {
                ProjectId = request.ProjectId,
                TaskId = request.TaskId,
                NewTitle = request.NewTitle,
                NewDescription = request.NewDescription,
                NewDueDate = request.NewDueDate,
                NewStatus = request.NewStatus,
                ModifiedBy = request.ModifiedBy
            };

            await _taskService.UpdateTaskAsync(updateTaskDto, cancellationToken);

            return Unit.Value;
        }
    }
}
