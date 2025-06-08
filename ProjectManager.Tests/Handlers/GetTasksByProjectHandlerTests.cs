using Moq;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.Tasks.Handlers;
using ProjectManager.Application.Tasks.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Tests.Handlers
{
    public class GetTasksByProjectHandlerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly GetTasksByProjectHandler _handler;

        public GetTasksByProjectHandlerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _handler = new GetTasksByProjectHandler(_taskServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTasksFromService()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var tasks = new List<Domain.Entities.Task>
        {
            new Domain.Entities.Task(Domain.Enums.TaskPriority.Low) { Id = Guid.NewGuid(), Title = "Task 1" },
            new Domain.Entities.Task(Domain.Enums.TaskPriority.Medium) { Id = Guid.NewGuid(), Title = "Task 2" }
        };

            _taskServiceMock
                .Setup(s => s.GetTasksByProjectAsync(projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(tasks);

            var query = new GetTasksByProjectQuery { ProjectId = projectId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(tasks, result);
            _taskServiceMock.Verify(s => s.GetTasksByProjectAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoTasks()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var emptyTasks = new List<Domain.Entities.Task>();

            _taskServiceMock
                .Setup(s => s.GetTasksByProjectAsync(projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyTasks);

            var query = new GetTasksByProjectQuery { ProjectId = projectId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
            _taskServiceMock.Verify(s => s.GetTasksByProjectAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
