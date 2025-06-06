using Core.Constants;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using Core.Utility;
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
        private readonly IReplyFactory _replyFactory;
        public ReplyService(IUnitOfWork unitOfWork,IReplyFactory replyFactory, IUserService userService, ILogger<ReplyService> logger, IHtmlSanitizer htmlSanitizer)
        {
            _unitOfWork = unitOfWork;
            _replyFactory = replyFactory;
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
        public async Task<List<ReplyItemDto>> GetListReplyItemDtoAsync(GetReplyItemRequest getReplyItemRequest)
        {
            return await _unitOfWork.Replies.GetListReplyItemDtoByUsernameAsync(getReplyItemRequest.Username);
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
            var post = await _unitOfWork.Posts.GetWithRepliesByIdAsync(createReplyRequest.PostId);
            if (post == null)
            {
                return PostResult.NotFound;
            }
            var reply = _replyFactory.Create(createReplyRequest.Content, user, post);
            string[] taggedUsernames = TextFormatter.CheckForTags(reply.Content);
            var notificationsList = new List<NotificationDataModel>();
            user.IncrementPostCount();
            RoleUtility.AdjustRoleByPostCount(user);
            if (taggedUsernames.Length > 0)
            {
                foreach(string taggedUsername in taggedUsernames)
                {
                    if(taggedUsername != UserConstants.DefaultUser.UserName)
                    {
                        var taggedUser = await _unitOfWork.Users.GetByUsernameAsync(taggedUsername);
                        if (taggedUser != null)
                        {
                            notificationsList.Add(new(reply,user,taggedUser));
                        }
                    }
                }
            }
            post.Replies.Add(reply);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (notificationsList.Count > 0)
                {
                    await _unitOfWork.Notifications.CreateRangeAsync(notificationsList);
                }
                await _unitOfWork.Replies.CreateAsync(reply);
                await _unitOfWork.Posts.UpdateAsync(post);
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex) 
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "There was a problem creating reply from user with Id: {UserId}", createReplyRequest.UserId);
                return PostResult.UpdateFailed;
            }
        }
    }
}
