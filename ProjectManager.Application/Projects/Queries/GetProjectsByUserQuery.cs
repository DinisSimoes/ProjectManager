using MediatR;
using ProjectManager.Application.Projects.Responses;

namespace ProjectManager.Application.Projects.Queries
{
    public class GetProjectsByUserQuery : IRequest<List<ProjectResponse>>
    {
        public Guid UserId {  get; set; }
    }
}
