using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public Task CommitAsync(CancellationToken token);
        public Task CommitTransactionAsync(CancellationToken token);
        Task RunPOSTOperationAsync<T>(Func<T, Task> func, T obj) where T : class;
        public Task RunPOSTOperationAsync<T>(Func<T, CancellationToken, Task> func, T obj, CancellationToken cancellationToken) where T : class;

        Task RunPOSTOperationAsync<T>(Action<T> func, T obj) where T : class;
        public Task RunPOSTOperationAsync<T>(Action<T, CancellationToken> func, T obj, CancellationToken token) where T : class;
        public Task RunPOSTOperationAsync<T>(Action<T> func, T obj, CancellationToken token) where T : class;
        Task RunPOSTOperationAsync<T1, T2>(Action<T1> func, T1 obj, Action<T2> func2, T2 obj2) where T1 : class where T2 : class;
        Task RunPOSTOperationAsync<T1, T2, T3>(Action<T1> func, T1 obj, Action<T2> func2, T2 obj2, Action<T3> func3, T3 obj3) where T1 : class where T2 : class where T3 : class;
        Task RunPOSTOperationAsync<T>(Action<List<T>> func, List<T> obj) where T : class;
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
