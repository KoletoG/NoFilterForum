using Core.Constants;
using Core.Enums;
using Core.Interfaces.Business_Logic;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Reply;
using Core.Models.DTOs.OutputDTOs.Reply;
using Core.Utility;
using Ganss.Xss;
using Microsoft.Extensions.Hosting;
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
        private readonly IReplyFactory _replyFactory;
        private readonly IReactionService _reactionService;
        public ReplyService(IUnitOfWork unitOfWork,
            IReactionService reactionService,
            IReplyFactory replyFactory,
            IUserService userService,
            ILogger<ReplyService> logger)
        {
            _unitOfWork = unitOfWork;
            _reactionService = reactionService;
            _replyFactory = replyFactory;
            _logger = logger;
            _userService = userService;
        }
        public async Task<PostResult> LikeAsync(LikeDislikeRequest likeDislikeRequest)
        {
            var user = await _userService.GetUserByIdAsync(likeDislikeRequest.UserId);
            if (user is null)
            {
                return PostResult.NotFound;
            }
            var reply = await _unitOfWork.Replies.GetByIdAsync(likeDislikeRequest.PostReplyId);
            if (reply is null)
            {
                return PostResult.NotFound;
            }
            _reactionService.ApplyLikeLogic(user, reply, likeDislikeRequest.PostReplyId);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.Users.Update(user);
                _unitOfWork.Replies.Update(reply);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Reply with Id: {ReplyId} was not liked", likeDislikeRequest.PostReplyId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<PostResult> DislikeAsync(LikeDislikeRequest likeDislikeRequest)
        {
            var user = await _userService.GetUserByIdAsync(likeDislikeRequest.UserId);
            if (user is null)
            {
                return PostResult.NotFound;
            }
            var reply = await _unitOfWork.Replies.GetByIdAsync(likeDislikeRequest.PostReplyId);
            if (reply is null)
            {
                return PostResult.NotFound;
            }
            _reactionService.ApplyDislikeLogic(user, reply, likeDislikeRequest.PostReplyId);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.Users.Update(user);
                _unitOfWork.Replies.Update(reply);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Reply with Id: {ReplyId} was not disliked", likeDislikeRequest.PostReplyId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<List<ReplyIndexItemDto>> GetListReplyIndexItemDto(GetListReplyIndexItemRequest getListReplyIndexItemRequest)
        {
            var repliesCount = await _unitOfWork.Replies.GetCountByPostIdAsync(getListReplyIndexItemRequest.PostId);
            if (repliesCount > 0)
            {
                return await _unitOfWork.Replies.GetReplyIndexItemDtoListByPostIdAndPageAsync(getListReplyIndexItemRequest.PostId, getListReplyIndexItemRequest.Page, PostConstants.PostsPerSection);
            }
            return new();
        }
        // Change this tomorrow
        public async Task<(int page, int totalPages)> GetPageAndTotalPage(int page, string postId, string replyId = "")
        {
            var repliesCount = await _unitOfWork.Replies.GetCountByPostIdAsync(postId);
            int totalPages = 1;
            if (repliesCount > 0)
            {
                totalPages = PageUtility.GetTotalPagesCount(repliesCount, PostConstants.PostsPerSection);
                if (string.IsNullOrEmpty(replyId))
                {
                    page = PageUtility.ValidatePageNumber(page, totalPages);
                }
                else
                {
                    var replyIds = await _unitOfWork.Replies.GetIdsByPostIdAsync(postId);
                    for (int i = 0; i < replyIds.Count; i++)
                    {
                        if (replyIds[i] == replyId)
                        {
                            break;
                        }
                        if ((i + 1) % PostConstants.PostsPerSection == 0)
                        {
                            page++;
                        }
                    }
                }
            }
            else
            {
                page = 1;
            }
            return (page, totalPages);
        }
        public async Task<bool> HasTimeoutByUserIdAsync(string userId)
        {
            var lastDateTime = await _unitOfWork.Replies.GetLastReplyDateTimeByUserIdAsync(userId);
            if (lastDateTime.AddSeconds(5) >= DateTime.UtcNow)
            {
                return !await _userService.IsAdminRoleByIdAsync(userId);
            }
            return false;
        }
        public async Task<List<ReplyItemDto>> GetListReplyItemDtoAsync(GetReplyItemRequest getReplyItemRequest) => await _unitOfWork.Replies.GetListReplyItemDtoByUserIdAsync(getReplyItemRequest.UserId);
        public async Task<PostResult> DeleteReplyAsync(DeleteReplyRequest request)
        {
            var reply = await _unitOfWork.Replies.GetWithUserByIdAsync(request.ReplyId);
            if (reply is null)
            {
                return PostResult.NotFound;
            }
            bool shouldForbid = reply.User.Id == request.UserId
                ? false
                : !(await _userService.IsAdminRoleByIdAsync(request.UserId));
            if (shouldForbid) return PostResult.Forbid;
            if (reply.User != UserConstants.DefaultUser)
            {
                reply.User.DecrementPostCount();
            }
            var notifications = await _unitOfWork.Notifications.GetAllByReplyIdAsync(request.ReplyId);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (notifications.Count > 0) _unitOfWork.Notifications.DeleteRange(notifications);
                _unitOfWork.Users.Update(reply.User);
                _unitOfWork.Replies.Delete(reply);
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
        private async Task<List<NotificationDataModel>> CreateNotificationsByTaggedUsernamesAsync(string[] taggedUsernames, ReplyDataModel reply, UserDataModel user)
        {
            var notificationsList = new List<NotificationDataModel>();
            string defaultUsername = UserConstants.DefaultUser.UserName ?? string.Empty;
            taggedUsernames = taggedUsernames.Where(x => x != defaultUsername).ToArray();
            var listOfTaggedUsers = await _unitOfWork.Users.GetListByUsernameArrayAsync(taggedUsernames);
            foreach(var taggedUser in listOfTaggedUsers)
            {
                notificationsList.Add(new(reply, user, taggedUser));
            }
            return notificationsList;
        }
        public async Task<PostResult> CreateReplyAsync(CreateReplyRequest createReplyRequest)
        {
            var user = await _userService.GetUserByIdAsync(createReplyRequest.UserId);
            if (user is null)
            {
                return PostResult.NotFound;
            }
            var post = await _unitOfWork.Posts.GetWithRepliesByIdAsync(createReplyRequest.PostId);
            if (post is null)
            {
                return PostResult.NotFound;
            }
            var reply = _replyFactory.Create(createReplyRequest.Content, user, post);
            string[] taggedUsernames = TextFormatter.CheckForTags(reply.Content);
            user.IncrementPostCount();
            var notificationsList = await CreateNotificationsByTaggedUsernamesAsync(taggedUsernames, reply, user);
            post.Replies.Add(reply);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (notificationsList.Count > 0)
                {
                    await _unitOfWork.Notifications.CreateRangeAsync(notificationsList);
                }
                await _unitOfWork.Replies.CreateAsync(reply);
                _unitOfWork.Posts.Update(post);
                _unitOfWork.Users.Update(user);
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
