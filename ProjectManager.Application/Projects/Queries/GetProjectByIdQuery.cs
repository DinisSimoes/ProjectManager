using MediatR;
using ProjectManager.Application.Projects.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Queries
{
    public class GetProjectByIdQuery : IRequest<ProjectResponse>
    {
        public Guid Id { get; set; }
    }
}
