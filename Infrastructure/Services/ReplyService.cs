using Core.Enums;
using Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class ReplyService : IReplyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReplyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task DeleteRepliesByUserAsync(UserDataModel user)
        {
            var replies = await _unitOfWork.Replies.GetAllByUserIdAsync(user.Id);
            if (replies.Count > 0)
            {
                user.PostsCount -= replies.Count;
                await _unitOfWork.Replies.DeleteRangeAsync(replies);
            }
        }
    }
}
