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
    public class AddCommentHandlerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly AddCommentHandler _handler;

        public AddCommentHandlerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _handler = new AddCommentHandler(_taskServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallAddCommentAsync_WithCorrectParameters()
        {
            // Arrange
            var command = new AddCommentCommand
            {
                ProjectId = Guid.NewGuid(),
                TaskId = Guid.NewGuid(),
                Comment = "Comentário teste",
                User = Guid.NewGuid()
            };

            _taskServiceMock
                .Setup(service => service.AddCommentAsync(
                    command.ProjectId,
                    command.TaskId,
                    command.Comment,
                    command.User,
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _taskServiceMock.Verify();
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ShouldPropagateException_WhenAddCommentAsyncThrows()
        {
            // Arrange
            var command = new AddCommentCommand
            {
                ProjectId = Guid.NewGuid(),
                TaskId = Guid.NewGuid(),
                Comment = "Comentário teste",
                User = Guid.NewGuid()
            };

            _taskServiceMock
                .Setup(service => service.AddCommentAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Erro ao adicionar comentário"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Erro ao adicionar comentário", exception.Message);
        }
    }
}
