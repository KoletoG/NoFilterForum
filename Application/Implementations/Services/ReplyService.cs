using Core.Constants;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
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
        public ReplyService(IUnitOfWork unitOfWork,IReplyFactory replyFactory, IUserService userService, ILogger<ReplyService> logger)
        {
            _unitOfWork = unitOfWork;
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
            var wasLiked = user.LikesPostRepliesIds.Contains(likeDislikeRequest.PostReplyId);
            var wasDisliked = user.DislikesPostRepliesIds.Contains(likeDislikeRequest.PostReplyId);
            if (wasDisliked)
            {
                reply.IncrementLikes();
                user.DislikesPostRepliesIds.Remove(likeDislikeRequest.PostReplyId);
            }
            if (wasLiked)
            {
                reply.DecrementLikes();
                user.LikesPostRepliesIds.Remove(likeDislikeRequest.PostReplyId);
            }
            else
            {
                reply.IncrementLikes();
                user.LikesPostRepliesIds.Add(likeDislikeRequest.PostReplyId);
            }
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
            var wasLiked = user.LikesPostRepliesIds.Contains(likeDislikeRequest.PostReplyId);
            var wasDisliked = user.DislikesPostRepliesIds.Contains(likeDislikeRequest.PostReplyId);
            if (wasLiked)
            {
                reply.DecrementLikes();
                user.LikesPostRepliesIds.Remove(likeDislikeRequest.PostReplyId);
            }
            if (wasDisliked)
            {
                reply.IncrementLikes();
                user.DislikesPostRepliesIds.Remove(likeDislikeRequest.PostReplyId);
            }
            else
            {
                reply.DecrementLikes();
                user.DislikesPostRepliesIds.Add(likeDislikeRequest.PostReplyId);
            }
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
                return await _unitOfWork.Replies.GetReplyIndexItemDtoListByPostIdAndPageAsync(getListReplyIndexItemRequest.PostId, getListReplyIndexItemRequest.Page,PostConstants.PostsPerSection);
            }
            return new();
        }
        public async Task<(int page,int totalPages)> GetPageAndTotalPage(int page, string postId, string replyId ="")
        {
            var repliesCount = await _unitOfWork.Replies.GetCountByPostIdAsync(postId);
            int totalPages=1;
            if (repliesCount>0)
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
        public void MarkTagsOfContents(ref List<ReplyIndexItemDto> replies, ref PostReplyIndexDto post, string currentUsername)
        {
            post.Content = TextFormatter.MarkTagsOfContent(post.Content, currentUsername);
            foreach (var reply in replies)
            {
                reply.Content = TextFormatter.MarkTagsOfContent(reply.Content, currentUsername); ;
            }
        }
        public async Task<bool> HasTimeoutByUserIdAsync(string userId)
        {
            var lastDateTime = await _unitOfWork.Replies.GetLastReplyDateTimeByUserIdAsync(userId);
            if (lastDateTime.AddSeconds(5) >= DateTime.UtcNow)
            {
                return await _userService.IsAdminRoleByIdAsync(userId);
            }
            return false;
        }
        public async Task<List<ReplyItemDto>> GetListReplyItemDtoAsync(GetReplyItemRequest getReplyItemRequest) => await _unitOfWork.Replies.GetListReplyItemDtoByUserIdAsync(getReplyItemRequest.UserId);
        public async Task<PostResult> DeleteReplyAsync(DeleteReplyRequest request)
        {
            var reply = await _unitOfWork.Replies.GetWithUserByIdAsync(request.ReplyId);
            if(reply is null)
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
                _unitOfWork.Notifications.DeleteRange(notifications);
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
        public async Task<PostResult> CreateReplyAsync(CreateReplyRequest createReplyRequest)
        {
            var user = await _userService.GetUserByIdAsync(createReplyRequest.UserId);
            if(user is null)
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
