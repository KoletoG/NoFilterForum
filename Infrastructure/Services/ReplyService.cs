using Core.Constants;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class ReplyService : IReplyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public ReplyService(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }
        public async Task<PostResult> DeleteReplyByIdAsync(DeleteReplyRequest request)
        {
            string replyUserId = await _unitOfWork.Replies.GetUserIdByReplyIdAsync(request.ReplyId);
            if (replyUserId != request.UserId)
            {
                bool isAdmin = await _userService.IsAdminRoleByIdAsync(request.UserId);
                if (!isAdmin)
                {
                    return PostResult.Forbid;
                }
            }
            var userOfReply = await _unitOfWork.Replies.GetUserByReplyIdAsync(replyUserId);
            if (userOfReply != UserConstants.DefaultUser)
            {
                userOfReply.DecrementPostCount();
            }
            var notifications = await _unitOfWork.Notifications.GetAllByReplyIdAsync(request.ReplyId);
            
            return PostResult.Success;
        }
    }
}
