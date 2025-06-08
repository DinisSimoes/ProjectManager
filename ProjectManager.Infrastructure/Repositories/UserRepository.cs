using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Interface;
using ProjectManager.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsManagerAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.IsManager)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
