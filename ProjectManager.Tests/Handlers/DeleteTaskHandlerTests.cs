using MediatR;
using Moq;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.Tasks.Commands;
using ProjectManager.Application.Tasks.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Tests.Handlers
{
    public class DeleteTaskHandlerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly DeleteTaskHandler _handler;

        public DeleteTaskHandlerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _handler = new DeleteTaskHandler(_taskServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallDeleteTaskAsync_WithCorrectParameters_AndReturnUnit()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var command = new DeleteTaskCommand
            {
                ProjectId = projectId,
                TaskId = taskId
            };

            _taskServiceMock
                .Setup(s => s.DeleteTaskAsync(projectId, taskId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _taskServiceMock.Verify();
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ShouldPropagateException_WhenDeleteTaskAsyncThrows()
        {
            // Arrange
            var command = new DeleteTaskCommand
            {
                ProjectId = Guid.NewGuid(),
                TaskId = Guid.NewGuid()
            };

            _taskServiceMock
                .Setup(s => s.DeleteTaskAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Erro ao deletar tarefa"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Erro ao deletar tarefa", exception.Message);
        }
    }
}
