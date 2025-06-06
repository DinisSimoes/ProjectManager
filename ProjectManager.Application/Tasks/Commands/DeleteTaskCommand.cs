using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Tasks.Commands
{
    public class DeleteTaskCommand : IRequest<Unit>
    {
        public Guid ProjectId { get; set; }
        public Guid TaskId { get; set; }
    }
}
