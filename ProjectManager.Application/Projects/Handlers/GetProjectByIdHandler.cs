using MediatR;
using ProjectManager.Application.Projects.Queries;
using ProjectManager.Application.Projects.Responses;
using ProjectManager.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Handlers
{
    public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectResponse>
    {
        private readonly IProjectRepository _repository;

        public GetProjectByIdHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProjectResponse> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (project == null)
                throw new KeyNotFoundException("Projeto não encontrado.");

            return new ProjectResponse
            {
                Id = project.Id,
                Name = project.Name,
                CreatedAt = project.CreatedAt
            };
        }
    }
}
