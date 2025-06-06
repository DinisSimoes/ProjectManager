using MediatR;

namespace ProjectManager.Application.Tasks.Queries
{
    public class GetTasksByProjectQuery : IRequest<IEnumerable<Domain.Entities.Task>>
    {
        public Guid ProjectId { get; set; }
    }
}
