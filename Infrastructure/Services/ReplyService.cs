using Core.Constants;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using Microsoft.Extensions.Logging;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class ReplyService : IReplyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ILogger<ReplyService> _logger;
        public ReplyService(IUnitOfWork unitOfWork, IUserService userService, ILogger<ReplyService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userService = userService;
        }
        public async Task<PostResult> DeleteReplyAsync(DeleteReplyRequest request)
        {
            var reply = await _unitOfWork.Replies.GetByIdAsync(request.ReplyId);
            if(reply == null)
            {
                return PostResult.NotFound;
            }
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
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Notifications.DeleteRangeAsync(notifications);
                await _unitOfWork.Users.UpdateAsync(userOfReply);
                await _unitOfWork.Replies.DeleteAsync(reply);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex) 
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Reply with Id: {ReplyId} wasn't deleted.", request.ReplyId);
                return PostResult.UpdateFailed;
            }
        }
    }
}
