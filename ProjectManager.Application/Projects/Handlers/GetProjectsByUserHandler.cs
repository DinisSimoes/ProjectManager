using MediatR;
using ProjectManager.Application.Projects.Queries;
using ProjectManager.Application.Projects.Responses;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interface;

namespace ProjectManager.Application.Projects.Handlers
{
    public class GetProjectsByUserHandler : IRequestHandler<GetProjectsByUserQuery, List<ProjectResponse>>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectsByUserHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<List<ProjectResponse>> Handle(GetProjectsByUserQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetByUserIdAsync(request.UserId);

            if (projects.Count == 0)
                throw new KeyNotFoundException("Sem projetos encontrados.");

            return projects.Select(p => new ProjectResponse
            {
                Id = p.Id,
                Name = p.Name,
                CreatedAt = p.CreatedAt
            }).ToList();
        }
    }
}
