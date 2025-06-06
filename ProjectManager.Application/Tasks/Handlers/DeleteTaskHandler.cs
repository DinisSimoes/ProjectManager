using MediatR;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.Tasks.Commands;

namespace ProjectManager.Application.Tasks.Handlers
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly ITaskService _taskService;

        public DeleteTaskHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            await _taskService.DeleteTaskAsync(request.ProjectId, request.TaskId, cancellationToken);
            return Unit.Value;
        }
    }
}
