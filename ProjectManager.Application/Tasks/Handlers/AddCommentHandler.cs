using MediatR;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.Tasks.Commands;

namespace ProjectManager.Application.Tasks.Handlers
{
    public class AddCommentHandler : IRequestHandler<AddCommentCommand, Unit>
    {
        private readonly ITaskService _taskService;

        public AddCommentHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<Unit> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            await _taskService.AddCommentAsync(request.ProjectId, request.TaskId, request.Comment, request.User, cancellationToken);
            return Unit.Value;
        }
    }
}
