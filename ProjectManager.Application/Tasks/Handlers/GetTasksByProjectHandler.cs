using MediatR;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.Tasks.Queries;

namespace ProjectManager.Application.Tasks.Handlers
{
    public class GetTasksByProjectHandler : IRequestHandler<GetTasksByProjectQuery, IEnumerable<Domain.Entities.Task>>
    {
        private readonly ITaskService _taskService;

        public GetTasksByProjectHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<IEnumerable<Domain.Entities.Task>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
        {
            return await _taskService.GetTasksByProjectAsync(request.ProjectId, cancellationToken);
        }
    }
}
