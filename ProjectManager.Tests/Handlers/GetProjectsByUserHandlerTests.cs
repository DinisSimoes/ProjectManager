using Moq;
using ProjectManager.Application.Projects.Handlers;
using ProjectManager.Application.Projects.Queries;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Tests.Handlers
{
    public class GetProjectsByUserHandlerTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly GetProjectsByUserHandler _handler;

        public GetProjectsByUserHandlerTests()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _handler = new GetProjectsByUserHandler(_projectRepositoryMock.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldReturnListOfProjectResponses_WhenProjectsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var projects = new List<Project>
        {
            new Project { Id = Guid.NewGuid(), Name = "Projeto 1", CreatedAt = DateTime.UtcNow, UserId = userId },
            new Project { Id = Guid.NewGuid(), Name = "Projeto 2", CreatedAt = DateTime.UtcNow, UserId = userId }
        };

            _projectRepositoryMock
                .Setup(repo => repo.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(projects);

            var query = new GetProjectsByUserQuery { UserId = userId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Name == "Projeto 1");
            Assert.Contains(result, r => r.Name == "Projeto 2");
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldThrowKeyNotFoundException_WhenNoProjectsFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetProjectsByUserQuery { UserId = userId };

            _projectRepositoryMock
                .Setup(repo => repo.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Project>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));

            Assert.Equal("Sem projetos encontrados.", exception.Message);
        }
    }
}
