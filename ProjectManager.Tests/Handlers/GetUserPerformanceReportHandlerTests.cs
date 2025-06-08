using Moq;
using ProjectManager.Application.PerformanceReport.Handlers;
using ProjectManager.Application.PerformanceReport.Queries;
using ProjectManager.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Tests.Handlers
{
    public class GetUserPerformanceReportHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPerformanceReportRepository> _performanceReportRepositoryMock;
        private readonly GetUserPerformanceReportHandler _handler;

        public GetUserPerformanceReportHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _performanceReportRepositoryMock = new Mock<IPerformanceReportRepository>();
            _handler = new GetUserPerformanceReportHandler(_userRepositoryMock.Object, _performanceReportRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenUserIsNotManager()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserPerformanceReportQuery() { RequestingUserId = userId };
            _userRepositoryMock.Setup(r => r.IsManagerAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldReturnPerformanceReport_WhenUserIsManager()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserPerformanceReportQuery() { RequestingUserId = userId };

            var performanceData = new Dictionary<Guid, double>
        {
            { Guid.NewGuid(), 5.4 },
            { Guid.NewGuid(), 3.1 }
        };

            _userRepositoryMock.Setup(r => r.IsManagerAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _performanceReportRepositoryMock
                .Setup(r => r.GetAverageCompletedTasksPerUserLast30DaysAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(performanceData);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(performanceData.Count, result.Count);

            foreach (var dto in result)
            {
                Assert.Contains(dto.UserId, performanceData.Keys);
                Assert.Equal(performanceData[dto.UserId], dto.AverageTasksCompletedLast30Days);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle_CallsIsManagerAsync_WithCorrectUserId(bool isManager)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var query = new GetUserPerformanceReportQuery() { RequestingUserId = userId };

            _userRepositoryMock
                .Setup(r => r.IsManagerAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(isManager);

            if (isManager)
            {
                _performanceReportRepositoryMock
                    .Setup(r => r.GetAverageCompletedTasksPerUserLast30DaysAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Dictionary<Guid, double>());
            }

            // Act
            try { await _handler.Handle(query, CancellationToken.None); } catch { /* ignore */ }

            // Assert
            _userRepositoryMock.Verify(r => r.IsManagerAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
