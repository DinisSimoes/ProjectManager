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
    public class GetProjectByIdHandlerTests
    {
        private readonly Mock<IProjectRepository> _repositoryMock;
        private readonly GetProjectByIdHandler _handler;

        public GetProjectByIdHandlerTests()
        {
            _repositoryMock = new Mock<IProjectRepository>();
            _handler = new GetProjectByIdHandler(_repositoryMock.Object);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldReturnProjectResponse_WhenProjectExists()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project
            {
                Id = projectId,
                Name = "Projeto Teste",
                CreatedAt = DateTime.UtcNow
            };

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(project);

            var query = new GetProjectByIdQuery { Id = projectId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(project.Id, result.Id);
            Assert.Equal(project.Name, result.Name);
            Assert.Equal(project.CreatedAt, result.CreatedAt);
        }

        [Fact]
        public async System.Threading.Tasks.Task Handle_ShouldThrowKeyNotFoundException_WhenProjectNotFound()
        {
            // Arrange
            var query = new GetProjectByIdQuery { Id = Guid.NewGuid() };

            _repositoryMock
                .Setup(repo => repo.GetByIdAsync(query.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Project)null!);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));

            Assert.Equal("Projeto não encontrado.", exception.Message);
        }
    }
}
