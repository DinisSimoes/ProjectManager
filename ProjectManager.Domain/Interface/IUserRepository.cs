﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Interface
{
    public interface IUserRepository
    {
        Task<bool> IsManagerAsync(Guid userId, CancellationToken cancellationToken);
    }
}
