using MediatR;
using Moq;
using ProjectManager.Application.DTOs;
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
    public class UpdateTaskHandlerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly UpdateTaskHandler _handler;

        public UpdateTaskHandlerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _handler = new UpdateTaskHandler(_taskServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallUpdateTaskAsync_WithCorrectParameters()
        {
            // Arrange
            var command = new UpdateTaskCommand
            {
                ProjectId = Guid.NewGuid(),
                TaskId = Guid.NewGuid(),
                NewTitle = "Updated Title",
                NewDescription = "Updated Description",
                NewDueDate = DateTime.UtcNow.AddDays(7),
                NewStatus = Domain.Enums.TaskStatus.Completed,
                ModifiedBy = Guid.NewGuid()
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _taskServiceMock.Verify(service =>
                service.UpdateTaskAsync(
                    It.Is<UpdateTaskDto>(dto =>
                        dto.ProjectId == command.ProjectId &&
                        dto.TaskId == command.TaskId &&
                        dto.NewTitle == command.NewTitle &&
                        dto.NewDescription == command.NewDescription &&
                        dto.NewDueDate == command.NewDueDate &&
                        dto.NewStatus == command.NewStatus &&
                        dto.ModifiedBy == command.ModifiedBy
                    ),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            Assert.Equal(Unit.Value, result);
        }
    }
}
