using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ApplicationDbContext context,
                          IPostRepository postRepository,
                          IReplyRepository replyRepository,
                          IUserRepository userRepository,
                          INotificationRepository notificationRepository,
                          IWarningRepository warningRepository,
                          ISectionRepository sectionRepository,
                          IReportRepository reportRepository)
        {
            _context = context;
            Posts = postRepository;
            Replies = replyRepository;
            Users = userRepository;
            Notifications = notificationRepository;
            Sections = sectionRepository;
            Reports = reportRepository;
            Warnings = warningRepository;
        }

        public IPostRepository Posts { get; }
        public IReplyRepository Replies { get; }
        public IUserRepository Users { get; }
        public IReportRepository Reports { get; }
        public INotificationRepository Notifications { get; }
        public ISectionRepository Sections { get; }
        public IWarningRepository Warnings { get; }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
        public async Task BeginTransactionAsync() => _transaction = await _context.Database.BeginTransactionAsync();
        public async Task RunPOSTOperationAsync<T>(Func<T, Task> func, T obj) where T : class
        {
            await BeginTransactionAsync();
            await func.Invoke(obj);
            await CommitAsync();
            await CommitTransactionAsync();
        }
        // If not used outside, make transaction methods private
        public async Task RunPOSTOperationAsync<T>(Action<T> func, T obj) where T : class
        {
            await BeginTransactionAsync();
            func.Invoke(obj);
            await CommitAsync();
            await CommitTransactionAsync();
        }
        public async Task RunPOSTOperationAsync<T1,T2>(Action<T1> func, T1 obj, Action<T2> func2, T2 obj2) where T1 : class where T2 : class
        {
            await BeginTransactionAsync();
            func.Invoke(obj);
            func2.Invoke(obj2);
            await CommitAsync();
            await CommitTransactionAsync();
        }
        public async Task RunPOSTOperationAsync<T1, T2,T3>(Action<T1> func, T1 obj, Action<T2> func2, T2 obj2, Action<T3> func3, T3 obj3) where T1 : class where T2 : class where T3 : class
        {
            await BeginTransactionAsync();
            func.Invoke(obj);
            func2.Invoke(obj2);
            func3.Invoke(obj3);
            await CommitAsync();
            await CommitTransactionAsync();
        }
        public async Task RunPOSTOperationAsync<T>(Action<List<T>> func, List<T> obj) where T : class
        {
            await BeginTransactionAsync();
            func.Invoke(obj);
            await CommitAsync();
            await CommitTransactionAsync();
        }
        public async Task CommitTransactionAsync() => await _transaction.CommitAsync();
        public async Task RollbackTransactionAsync() => await _transaction.RollbackAsync();
    }
}
