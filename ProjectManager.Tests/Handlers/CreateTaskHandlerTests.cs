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
    public class CreateTaskHandlerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly CreateTaskHandler _handler;

        public CreateTaskHandlerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _handler = new CreateTaskHandler(_taskServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallCreateTaskAsync_WithCorrectParameters_AndReturnTask()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var taskCommand = new CreateTaskCommand
            {
                ProjectId = projectId,
                Title = "Nova tarefa",
                Description = "Descrição da tarefa",
                DueDate = DateTime.UtcNow.AddDays(7),
                Priority = (Domain.Enums.TaskPriority)1,
                ResponsibleUserId = Guid.NewGuid()
            };

            var createdTask = new Domain.Entities.Task(taskCommand.Priority)
            {
                Id = Guid.NewGuid(),
                Title = taskCommand.Title,
                Description = taskCommand.Description,
                DueDate = taskCommand.DueDate,
                ResponsibleUserId = taskCommand.ResponsibleUserId
            };

            _taskServiceMock
                .Setup(s => s.CreateTaskAsync(
                    projectId,
                    It.Is<CreateTaskDto>(dto =>
                        dto.Title == taskCommand.Title &&
                        dto.Description == taskCommand.Description &&
                        dto.DueDate == taskCommand.DueDate &&
                        dto.Priority == taskCommand.Priority &&
                        dto.ResponsibleUserId == taskCommand.ResponsibleUserId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdTask);

            // Act
            var result = await _handler.Handle(taskCommand, CancellationToken.None);

            // Assert
            _taskServiceMock.VerifyAll();
            Assert.NotNull(result);
            Assert.Equal(createdTask.Id, result.Id);
            Assert.Equal(createdTask.Title, result.Title);
            Assert.Equal(createdTask.Description, result.Description);
            Assert.Equal(createdTask.DueDate, result.DueDate);
            Assert.Equal(createdTask.Priority, result.Priority);
            Assert.Equal(createdTask.ResponsibleUserId, result.ResponsibleUserId);
        }

        [Fact]
        public async Task Handle_ShouldPropagateException_WhenCreateTaskAsyncThrows()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                ProjectId = Guid.NewGuid(),
                Title = "Tarefa teste",
                Description = "Descrição",
                DueDate = DateTime.UtcNow.AddDays(1),
                Priority = (Domain.Enums.TaskPriority)2,
                ResponsibleUserId = Guid.NewGuid()
            };

            _taskServiceMock
                .Setup(s => s.CreateTaskAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CreateTaskDto>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Erro ao criar tarefa"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Erro ao criar tarefa", exception.Message);
        }
    }
}
