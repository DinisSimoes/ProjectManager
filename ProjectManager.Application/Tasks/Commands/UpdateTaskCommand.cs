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
    public class UpdateTaskCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        [JsonIgnore]
        public Guid TaskId { get; set; }
        public string? NewTitle { get; set; }
        public string? NewDescription { get; set; }
        public DateTime? NewDueDate { get; set; }
        public Domain.Enums.TaskStatus? NewStatus { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
