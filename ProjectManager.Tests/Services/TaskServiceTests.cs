using Moq;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Enums;
using ProjectManager.Domain.Exceptions;
using ProjectManager.Domain.Interface;

namespace ProjectManager.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly TaskService _taskService;
        private readonly CancellationToken _ct = CancellationToken.None;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object, _projectRepositoryMock.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTasksByProjectAsync_ShouldReturnTasks_WhenTasksExist()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var tasks = new List<Domain.Entities.Task> { new Domain.Entities.Task(TaskPriority.Medium) { Id = Guid.NewGuid() } };
            _taskRepositoryMock.Setup(r => r.GetTasksByProjectAsync(projectId, _ct)).ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetTasksByProjectAsync(projectId, _ct);

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTasksByProjectAsync_ShouldThrowNotFound_WhenNoTasks()
        {
            var projectId = Guid.NewGuid();
            _taskRepositoryMock.Setup(r => r.GetTasksByProjectAsync(projectId, _ct)).ReturnsAsync(Enumerable.Empty<Domain.Entities.Task>());

            await Assert.ThrowsAsync<NotFoundException>(() => _taskService.GetTasksByProjectAsync(projectId, _ct));
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTaskAsync_ShouldCreateTask_WhenProjectExistsAndUnderLimit()
        {
            var projectId = Guid.NewGuid();
            var dto = new CreateTaskDto
            {
                Title = "New Task",
                Description = "Description",
                DueDate = DateTime.Today.AddDays(5),
                Priority = TaskPriority.High,
                ResponsibleUserId = Guid.NewGuid()
            };

            _projectRepositoryMock.Setup(r => r.GetByIdAsync(projectId, _ct)).ReturnsAsync(new Project());
            _taskRepositoryMock.Setup(r => r.GetTasksByProjectAsync(projectId, _ct)).ReturnsAsync(new List<Domain.Entities.Task>());

            // Act
            var task = await _taskService.CreateTaskAsync(projectId, dto, _ct);

            // Assert
            Assert.Equal(dto.Title, task.Title);
            _taskRepositoryMock.Verify(r => r.AddTaskAsync(It.IsAny<Domain.Entities.Task>(), _ct), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTaskAsync_ShouldThrow_WhenProjectDoesNotExist()
        {
            var projectId = Guid.NewGuid();
            var dto = new CreateTaskDto();
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(projectId, _ct)).ReturnsAsync((Project)null);

            await Assert.ThrowsAsync<ArgumentException>(() => _taskService.CreateTaskAsync(projectId, dto, _ct));
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTaskAsync_ShouldThrow_WhenTaskLimitExceeded()
        {
            var projectId = Guid.NewGuid();
            var dto = new CreateTaskDto();
            var tasks = Enumerable.Range(1, 20).Select(_ => new Domain.Entities.Task(TaskPriority.Low)).ToList();

            _projectRepositoryMock.Setup(r => r.GetByIdAsync(projectId, _ct)).ReturnsAsync(new Project());
            _taskRepositoryMock.Setup(r => r.GetTasksByProjectAsync(projectId, _ct)).ReturnsAsync(tasks);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _taskService.CreateTaskAsync(projectId, dto, _ct));
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskAsync_ShouldCallDelete_WhenTaskExists()
        {
            var projectId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var task = new Domain.Entities.Task(TaskPriority.Low) { Id = taskId };

            _taskRepositoryMock.Setup(r => r.GetTaskByIdAsync(projectId, taskId, _ct)).ReturnsAsync(task);

            await _taskService.DeleteTaskAsync(projectId, taskId, _ct);

            _taskRepositoryMock.Verify(r => r.DeleteTaskAsync(task, _ct), Times.Once);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskAsync_ShouldThrow_WhenTaskNotFound()
        {
            var projectId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            _taskRepositoryMock.Setup(r => r.GetTaskByIdAsync(projectId, taskId, _ct)).ReturnsAsync((Domain.Entities.Task)null);

            await Assert.ThrowsAsync<ArgumentException>(() => _taskService.DeleteTaskAsync(projectId, taskId, _ct));
        }

        [Fact]
        public async System.Threading.Tasks.Task AddCommentAsync_ShouldUpdateTask_WhenTaskExists()
        {
            var projectId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var task = new Domain.Entities.Task(TaskPriority.Medium) { Id = taskId };

            _taskRepositoryMock.Setup(r => r.GetTaskByIdAsync(projectId, taskId, _ct)).ReturnsAsync(task);

            await _taskService.AddCommentAsync(projectId, taskId, "Comentario", userId, _ct);

            _taskRepositoryMock.Verify(r => r.UpdateTaskAsync(task, _ct), Times.Once);
        }

        [Theory]
        [InlineData("Novo Título", "Nova descrição", null)]
        [InlineData(null, null, "2025-06-10")]
        public async System.Threading.Tasks.Task UpdateTaskAsync_ShouldUpdateFields(string newTitle, string newDescription, string newDate)
        {
            var projectId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var modifiedBy = Guid.NewGuid();
            var task = new Domain.Entities.Task(TaskPriority.Low)
            {
                Id = taskId,
                Title = "Antigo",
                Description = "Desc",
                DueDate = DateTime.Today
            };

            _taskRepositoryMock.Setup(r => r.GetTaskByIdAsync(projectId, taskId, _ct)).ReturnsAsync(task);

            var dto = new UpdateTaskDto
            {
                TaskId = taskId,
                ProjectId = projectId,
                NewTitle = newTitle,
                NewDescription = newDescription,
                NewDueDate = newDate is not null ? DateTime.Parse(newDate) : (DateTime?)null,
                ModifiedBy = modifiedBy
            };

            await _taskService.UpdateTaskAsync(dto, _ct);

            _taskRepositoryMock.Verify(r => r.UpdateTaskAsync(task, _ct), Times.Once);
        }
    }
}
