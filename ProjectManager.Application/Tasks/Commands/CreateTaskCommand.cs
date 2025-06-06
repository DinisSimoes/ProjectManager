using MediatR;
using ProjectManager.Domain.Enums;
using System.Text.Json.Serialization;

namespace ProjectManager.Application.Tasks.Commands
{
    public class CreateTaskCommand : IRequest<Domain.Entities.Task>
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public Guid ResponsibleUserId { get; set; }
    }
}
