using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Tasks.Commands;
using ProjectManager.Application.Tasks.Queries;

namespace ProjectManager.API.Controllers
{
    [ApiController]
    [Route("projects/{projectId:guid}/tasks")]
    public class TasksController : Controller
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksByProject(Guid projectId)
        {
            var query = new GetTasksByProjectQuery { ProjectId = projectId };
            var tasks = await _mediator.Send(query);
            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(Guid projectId, [FromBody] CreateTaskCommand command)
        {
            command.ProjectId = projectId;
            var task = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTasksByProject), new { projectId }, task);
        }

        [HttpPut("{taskId:guid}")]
        public async Task<IActionResult> UpdateTask(Guid projectId, Guid taskId, [FromBody] UpdateTaskCommand command)
        {
            command.ProjectId = projectId;
            command.TaskId = taskId;
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{taskId:guid}")]
        public async Task<IActionResult> DeleteTask(Guid projectId, Guid taskId)
        {
            var command = new DeleteTaskCommand
            {
                ProjectId = projectId,
                TaskId = taskId
            };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("{taskId:guid}/comments")]
        public async Task<IActionResult> AddComment(Guid projectId, Guid taskId, [FromBody] AddCommentCommand command)
        {
            command.ProjectId = projectId;
            command.TaskId = taskId;
            await _mediator.Send(command);
            return Ok();
        }
    }
}
