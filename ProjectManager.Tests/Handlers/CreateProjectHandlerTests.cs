using Moq;
using ProjectManager.Application.Projects.Commands;
using ProjectManager.Application.Projects.Handlers;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Tests.Handlers
{
    public class CreateProjectHandlerTests
    {
        private readonly Mock<IProjectRepository> _repositoryMock;
        private readonly CreateProjectHandler _handler;

        public CreateProjectHandlerTests()
        {
            _repositoryMock = new Mock<IProjectRepository>();
            _handler = new CreateProjectHandler(_repositoryMock.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldCallAddAsync_WithCorrectProject()
        {
            // Arrange
            var command = new CreateProjectCommand
            {
                UserId = Guid.NewGuid(),
                Name = "Novo Projeto"
            };

            Project? savedProject = null;

            _repositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
                .Callback<Project, CancellationToken>((proj, _) => savedProject = proj)
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.NotNull(savedProject);
            Assert.Equal(command.UserId, savedProject.UserId);
            Assert.Equal(command.Name, savedProject.Name);
            Assert.True((DateTime.UtcNow - savedProject.CreatedAt).TotalSeconds < 5);

            Assert.Equal(savedProject.Id, result.Id);
            Assert.Equal(savedProject.Name, result.Name);
            Assert.Equal(savedProject.CreatedAt, result.CreatedAt);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldReturnCorrectProjectResponse()
        {
            // Arrange
            var command = new CreateProjectCommand
            {
                UserId = Guid.NewGuid(),
                Name = "Projeto XPTO"
            };

            _repositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
                .Returns(System.Threading.Tasks.Task.CompletedTask);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, response.Id);
            Assert.Equal(command.Name, response.Name);
            Assert.True((DateTime.UtcNow - response.CreatedAt).TotalSeconds < 5);
        }
    }
}
