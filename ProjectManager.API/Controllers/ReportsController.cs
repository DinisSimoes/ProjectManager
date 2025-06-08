using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.PerformanceReport.Queries;

namespace ProjectManager.API.Controllers
{
    [ApiController]
    [Route("reports")]
    public class ReportsController : Controller
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("user-performance")]
        public async Task<IActionResult> GetUserPerformanceReport([FromQuery] Guid userId)
        {
            try
            {
                var result = await _mediator.Send(new GetUserPerformanceReportQuery { RequestingUserId=userId});
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}
