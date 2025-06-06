using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Interface
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        System.Threading.Tasks.Task AddAsync(Project project, CancellationToken cancellationToken);
        Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
