using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Projects.Queries;

namespace ProjectManager.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId:guid}/projects")]
        public async Task<IActionResult> GetProjectsByUser([FromRoute] Guid userId)
        {
            var request = new GetProjectsByUserQuery { UserId = userId };
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
