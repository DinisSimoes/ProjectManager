using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs
{
    public class UserPerformanceReportDto
    {
        public Guid UserId { get; set; }
        public double AverageTasksCompletedLast30Days { get; set; }
    }
}
