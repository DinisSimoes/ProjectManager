using MediatR;
using ProjectManager.Application.DTOs;
using ProjectManager.Application.PerformanceReport.Queries;
using ProjectManager.Domain.Interface;

namespace ProjectManager.Application.PerformanceReport.Handlers
{
    public class GetUserPerformanceReportHandler : IRequestHandler<GetUserPerformanceReportQuery, List<UserPerformanceReportDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPerformanceReportRepository _performanceReportRepository;

        public GetUserPerformanceReportHandler(IUserRepository userRepository, IPerformanceReportRepository performanceReportRepository)
        {
            _userRepository = userRepository;
            _performanceReportRepository = performanceReportRepository;
        }

        public async Task<List<UserPerformanceReportDto>> Handle(GetUserPerformanceReportQuery request, CancellationToken cancellationToken)
        {
            var isManager = await _userRepository.IsManagerAsync(request.RequestingUserId, cancellationToken);

            if (!isManager)
            {
                throw new InvalidOperationException("Usuário sem permissões");
            }

            var averages = await _performanceReportRepository.GetAverageCompletedTasksPerUserLast30DaysAsync(cancellationToken);

            return averages
                .Select(x => new UserPerformanceReportDto
                {
                    UserId = x.Key,
                    AverageTasksCompletedLast30Days = x.Value
                })
                .ToList();
        }
    }
}
