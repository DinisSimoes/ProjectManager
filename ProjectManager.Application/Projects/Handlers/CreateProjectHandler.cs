using MediatR;
using ProjectManager.Application.Projects.Commands;
using ProjectManager.Application.Projects.Responses;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interface;

namespace ProjectManager.Application.Projects.Handlers
{
    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, ProjectResponse>
    {
        private readonly IProjectRepository _repository;

        public CreateProjectHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProjectResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Name = request.Name,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(project, cancellationToken);

            return new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                CreatedAt = project.CreatedAt
            };
        }
    }
}
