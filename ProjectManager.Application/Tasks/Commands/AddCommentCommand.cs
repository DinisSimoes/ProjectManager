using MediatR;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectManager.Application.Tasks.Commands
{
    public class AddCommentCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        [JsonIgnore]
        public Guid TaskId { get; set; }
        public string Comment { get; set; } = null!;
        public Guid User { get; set; }
    }
}
