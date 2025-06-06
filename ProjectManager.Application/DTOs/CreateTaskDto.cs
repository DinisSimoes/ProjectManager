using ProjectManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public Guid ResponsibleUserId { get; set; }
    }
}
