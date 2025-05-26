using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFilterForum.Core.Interfaces.Repositories;

namespace Core.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IPostRepository Posts { get; }
        IReplyRepository Replies { get; }
        IUserRepository Users { get; }
        ISectionRepository Sections { get; }
        INotificationRepository Notifications { get; }
        IReportRepository Reports { get; }
        IWarningRepository Warnings { get; }
        Task<int> CommitAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
