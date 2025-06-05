using MediatR;
using ProjectManager.Application.Projects.Responses;

namespace ProjectManager.Application.Projects.Commands
{
    public class CreateProjectCommand : IRequest<ProjectResponse>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
