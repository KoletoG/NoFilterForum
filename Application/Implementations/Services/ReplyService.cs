using Application.Interfaces.Services;
using Core.Constants;
using Core.DTOs.InputDTOs.Reply;
using Core.DTOs.OutputDTOs.Reply;
using Core.Enums;
using Core.Implementations.Services;
using Core.Interfaces.Business_Logic;
using Core.Interfaces.Factories;
using Core.Interfaces.Hub;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Reply;
using Core.Models.DTOs.OutputDTOs.Reply;
using Core.Utility;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace Application.Implementations.Services
{
    public class ReplyService : IReplyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ILogger<ReplyService> _logger;
        private readonly IReplyFactory _replyFactory;
        private readonly ICacheService _cacheService;
        private readonly INotificationHub _notificationHub;
        public ReplyService(IUnitOfWork unitOfWork,
            IReplyFactory replyFactory,
            IUserService userService,
            ILogger<ReplyService> logger,
            ICacheService cacheService,
            INotificationHub notificationHub)
        {
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
            _replyFactory = replyFactory;
            _logger = logger;
            _userService = userService;
            _notificationHub = notificationHub;
        }
        // GET methods
        public async Task<IReadOnlyCollection<ReplyIndexItemDto>> GetListReplyIndexItemDto(GetListReplyIndexItemRequest getListReplyIndexItemRequest, CancellationToken cancellationToken) => await _cacheService.TryGetValue($"replyIndexItemsDtoById_{getListReplyIndexItemRequest.PostId}_Page_{getListReplyIndexItemRequest.Page}", _unitOfWork.Replies.GetReplyIndexItemDtoListByPostIdAndPageAsync, getListReplyIndexItemRequest, cancellationToken) ?? [];
        public async Task<PageTotalPagesDTO> GetPageTotalPagesDTOByReplyIdAndPostIdAsync(string replyId, string postId, CancellationToken cancellationToken)
        {
            int totalPages = await GetTotalPagesByPostIdAsync(postId, cancellationToken);
            if (totalPages == 1) return new(1, 1);

            int page = 1;
            var replyIds = await _unitOfWork.Replies.GetIdsByPostIdAsync(postId, cancellationToken);
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
            return new(page, totalPages);
        }
        private async Task<int> GetTotalPagesByPostIdAsync(string postId, CancellationToken cancellationToken)
        {
            var repliesCount = await _unitOfWork.Replies.GetCountByPostIdAsync(postId,cancellationToken);
            if (repliesCount == 0) return 1;
            int totalPages = PageUtility.GetTotalPagesCount(repliesCount, PostConstants.PostsPerSection);
            return totalPages;
        }
        public async Task<PageTotalPagesDTO> GetPageAndTotalPagesDTOByPostIdAsync(string postId, int page, CancellationToken cancellationToken)
        {
            int totalPages = await GetTotalPagesByPostIdAsync(postId, cancellationToken);
            if (totalPages == 1) return new(1, 1);
            page = PageUtility.ValidatePageNumber(page, totalPages);
            return new(page, totalPages);
        }
        public async Task<bool> HasTimeoutByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            var lastDateTime = await _unitOfWork.Replies.GetLastReplyDateTimeByUserIdAsync(userId, cancellationToken);
            if (lastDateTime.AddSeconds(5) >= DateTime.UtcNow)
            {
                return !await _userService.IsAdminOrVIPAsync(userId);
            }
            return false;
        }
        public async Task<IDictionary<string, ReplyItemDto>> GetListReplyItemDtoAsync(string userId, CancellationToken cancellationToken) => await _cacheService.TryGetValue($"listReplyItemDtoById_{userId}", _unitOfWork.Replies.GetListReplyItemDtoByUserIdAsync, userId, cancellationToken);
        // POST methods
        public async Task<PostResult> LikeAsync(LikeDislikeRequest likeDislikeRequest, CancellationToken cancellationToken)
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
            ReactionUtility.ApplyLikeLogic(user, reply, likeDislikeRequest.PostReplyId);
            try
            {
                await _unitOfWork.RunPOSTOperationAsync(_unitOfWork.Replies.Update, reply, _unitOfWork.Users.Update, user, cancellationToken);
                return PostResult.Success;
            }
            catch(OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Liking of reply with Id: {ReplyId} was cancelled", likeDislikeRequest.PostReplyId);
                return PostResult.UpdateFailed;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Reply with Id: {ReplyId} was not liked", likeDislikeRequest.PostReplyId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<PostResult> DislikeAsync(LikeDislikeRequest likeDislikeRequest, CancellationToken cancellationToken)
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
            ReactionUtility.ApplyDislikeLogic(user, reply, likeDislikeRequest.PostReplyId);
            try
            {
                await _unitOfWork.RunPOSTOperationAsync(_unitOfWork.Users.Update, user, _unitOfWork.Replies.Update, reply, cancellationToken);
                return PostResult.Success;
            }
            catch (OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Disliking of reply with Id: {ReplyId} was cancelled", likeDislikeRequest.PostReplyId);
                return PostResult.UpdateFailed;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Reply with Id: {ReplyId} was not disliked", likeDislikeRequest.PostReplyId);
                return PostResult.UpdateFailed;
            }
        }
        
        public async Task<PostResult> DeleteReplyAsync(DeleteReplyRequest request, CancellationToken cancellationToken)
        {
            var reply = await _unitOfWork.Replies.GetWithUserByIdAsync(request.ReplyId, cancellationToken);
            if (reply is null)
            {
                return PostResult.NotFound;
            }
            bool shouldForbid = reply.UserId != request.UserId && !await _userService.IsAdminAsync(request.UserId);
            if (shouldForbid) return PostResult.Forbid;
            if (reply.User != UserConstants.DefaultUser)
            {
                reply.User.DecrementPostCount();
            }
            var notifications = await _unitOfWork.Notifications.GetAllByReplyIdAsync(request.ReplyId, cancellationToken);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (notifications.Any()) _unitOfWork.Notifications.DeleteRange(notifications);
                await _userService.ApplyRoleAsync(reply.User); // CHANGE THAT
                _unitOfWork.Users.Update(reply.User);
                _unitOfWork.Replies.Delete(reply);
                await _unitOfWork.CommitAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return PostResult.Success;
            }
            catch (OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Deleting reply with Id: {ReplyId} was cancelled", request.ReplyId);
                return PostResult.UpdateFailed;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Reply with Id: {ReplyId} wasn't deleted.", request.ReplyId);
                return PostResult.UpdateFailed;
            }
        }
        private async Task<IEnumerable<NotificationDataModel>> CreateNotificationsByTaggedUsernamesAsync(IEnumerable<string> taggedUsernames, ReplyDataModel reply, UserDataModel user, CancellationToken cancellationToken)
        {
            string defaultUsername = UserConstants.DefaultUser.UserName ?? string.Empty;
            var taggedUsernamesList = taggedUsernames.Where(x => x != defaultUsername).ToList();
            var listOfTaggedUsers = await _unitOfWork.Users.GetListByUsernameArrayAsync(taggedUsernamesList, cancellationToken);
            var notificationsList = listOfTaggedUsers.Select(x=>new NotificationDataModel(reply,user,x)); 
            await _notificationHub.SendNotificationAsync(listOfTaggedUsers.Select(x=>x.Id));
            return notificationsList;
        }
        public async Task<PostResult> CreateReplyAsync(CreateReplyRequest createReplyRequest, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(createReplyRequest.UserId);
            if (user is null)
            {
                return PostResult.NotFound;
            }
            var post = await _unitOfWork.Posts.GetWithRepliesByIdAsync(createReplyRequest.PostId, cancellationToken);
            if (post is null)
            {
                return PostResult.NotFound;
            }
            var reply = _replyFactory.Create(createReplyRequest.Content, user, post);
            string[] taggedUsernames = TextFormatter.CheckForTags(reply.Content);
            var notificationsList = await CreateNotificationsByTaggedUsernamesAsync(taggedUsernames, reply, user, cancellationToken);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (notificationsList.Any())
                {
                    await _unitOfWork.Notifications.CreateRangeAsync(notificationsList,cancellationToken);
                }
                await _unitOfWork.Replies.CreateAsync(reply, cancellationToken);
                _unitOfWork.Posts.Update(post);
                await _userService.ApplyRoleAsync(user); // CHANGE THAT
                _unitOfWork.Users.Update(user);
                await _unitOfWork.CommitAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return PostResult.Success;
            }
            catch (OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Creating reply from user with Id: {UserId} was cancelled", createReplyRequest.UserId);
                return PostResult.UpdateFailed;
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
