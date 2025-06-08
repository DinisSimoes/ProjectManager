using MediatR;
using ProjectManager.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.PerformanceReport.Queries
{
    public class GetUserPerformanceReportQuery : IRequest<List<UserPerformanceReportDto>>
    {
        public Guid RequestingUserId { get; set; }
    }
}
