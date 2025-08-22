/// <summary>
/// The IUnitOfWork interface defines a contract for managing database transactions and saving changes as a single unit.
/// It is commonly used in repository patterns to coordinate the work of multiple repositories by committing changes together.
/// </summary
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCenterProject.Infrastructure.Abstract
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}