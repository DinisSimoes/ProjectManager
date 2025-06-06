using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs
{
    public class UpdateTaskDto
    {
        public Guid ProjectId { get; set; }
        public Guid TaskId { get; set; }
        public string NewTitle { get; set; }
        public string NewDescription { get; set; }
        public DateTime? NewDueDate { get; set; }
        public Domain.Enums.TaskStatus? NewStatus { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
