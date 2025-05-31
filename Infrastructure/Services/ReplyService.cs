using Core.Constants;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using Ganss.Xss;
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
        private readonly IHtmlSanitizer _htmlSanitizer;
        public ReplyService(IUnitOfWork unitOfWork, IUserService userService, ILogger<ReplyService> logger, IHtmlSanitizer htmlSanitizer)
        {
            _unitOfWork = unitOfWork;
            _htmlSanitizer = htmlSanitizer;
            _htmlSanitizer.AllowedTags.Clear();
            _htmlSanitizer.AllowedTags.Add("a");
            _logger = logger;
            _userService = userService;
        }
        public async Task<bool> HasTimeoutByUserIdAsync(string userId)
        {
            var lastDateTime = await _unitOfWork.Replies.GetLastReplyDateTimeByUserIdAsync(userId);
            if (lastDateTime.AddSeconds(5) >= DateTime.UtcNow)
            {
                if (await _userService.IsAdminRoleByIdAsync(userId))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public async Task<PostResult> DeleteReplyAsync(DeleteReplyRequest request)
        {
            var reply = await _unitOfWork.Replies.GetWithUserByIdAsync(request.ReplyId);
            if(reply == null)
            {
                return PostResult.NotFound;
            }
            if (reply.User.Id != request.UserId)
            {
                bool isAdmin = await _userService.IsAdminRoleByIdAsync(request.UserId);
                if (!isAdmin)
                {
                    return PostResult.Forbid;
                }
            }
            if (reply.User != UserConstants.DefaultUser)
            {
                reply.User.DecrementPostCount();
            }
            var notifications = await _unitOfWork.Notifications.GetAllByReplyIdAsync(request.ReplyId);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Notifications.DeleteRangeAsync(notifications);
                await _unitOfWork.Users.UpdateAsync(reply.User);
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
        public async Task<PostResult> CreateReplyAsync(CreateReplyRequest createReplyRequest)
        {
            var user = await _userService.GetUserByIdAsync(createReplyRequest.UserId);
            if(user == null)
            {
                return PostResult.NotFound;
            }
            string sanitizedContent =_htmlSanitizer.Sanitize(createReplyRequest.Content);
            return PostResult.Success; 
        }
    }
}
