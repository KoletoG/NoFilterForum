using System.Runtime.CompilerServices;
using System.Web;
using Application.Interfaces.Services;
using Core.Constants;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Post;
using Core.Models.DTOs.InputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Post;
using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using Core.Utility;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{


    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ILogger<PostService> _logger;
        private readonly IPostFactory _postFactory;
        private readonly IReactionService _reactionService;
        private readonly ICacheService _cacheService;
        public PostService(IUnitOfWork unitOfWork,ICacheService cacheService, IReactionService reactionService, IUserService userService, ILogger<PostService> logger, IPostFactory postFactory)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _reactionService = reactionService;
            _logger = logger;
            _userService = userService;
            _postFactory = postFactory;
        }
        public async Task<PostResult> LikeAsync(LikeDislikeRequest likeDislikeRequest)
        {
            var user = await _userService.GetUserByIdAsync(likeDislikeRequest.UserId);
            if (user is null)
            {
                return PostResult.NotFound;
            }
            var post = await _unitOfWork.Posts.GetByIdAsync(likeDislikeRequest.PostReplyId);
            if (post is null)
            {
                return PostResult.NotFound;
            }
            _reactionService.ApplyLikeLogic(user, post, likeDislikeRequest.PostReplyId);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.Users.Update(user);
                _unitOfWork.Posts.Update(post);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Post with Id: {PostId} was not liked", likeDislikeRequest.PostReplyId);
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
            var post = await _unitOfWork.Posts.GetByIdAsync(likeDislikeRequest.PostReplyId);
            if (post is null)
            {
                return PostResult.NotFound;
            }
            _reactionService.ApplyDislikeLogic(user, post, likeDislikeRequest.PostReplyId);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.Users.Update(user);
                _unitOfWork.Posts.Update(post);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Post with Id: {PostId} was not disliked", likeDislikeRequest.PostReplyId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<string?> GetSectionTitleByPostIdAsync(string postId) => await _unitOfWork.Posts.GetSectionTitleByIdAsync(postId);
        public async Task<string?> GetPostIdByReplyId(string replyId) => await _unitOfWork.Replies.GetPostIdById(replyId);
        public async Task<PostReplyIndexDto?> GetPostReplyIndexDtoByIdAsync(string postId) => await _unitOfWork.Posts.GetPostReplyIndexDtoByIdAsync(postId);
        public async Task<List<ProfilePostDto>> GetListProfilePostDtoAsync(GetProfilePostDtoRequest getProfilePostDtoRequest) => await _unitOfWork.Posts.GetListProfilePostDtoByUserIdAsync(getProfilePostDtoRequest.UserId);
        public async Task<PostResult> PinPostAsync(string postId)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(postId);
            if (post is null)
            {
                return PostResult.NotFound;
            }
            post.TogglePin();
            try
            {
                await _unitOfWork.RunPOSTOperationAsync<PostDataModel>(_unitOfWork.Posts.Update, post);
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Problem (un)pinning post with ID: {PostId}.", postId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<bool> HasTimeoutAsync(string userId)
        {
            var dateOfLastPost = await _unitOfWork.Posts.GetLastPostDateByUsernameAsync(userId);
            if (dateOfLastPost.AddSeconds(5) >= DateTime.UtcNow)
            {
                return !await _userService.IsAdminRoleByIdAsync(userId);
            }
            return false;
        }
        public async Task<PostResult> CreatePostAsync(CreatePostRequest createPost)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(createPost.UserId);
            if (user is null)
            {
                return PostResult.NotFound;
            }
            var section = await _unitOfWork.Sections.GetWithPostsByTitleAsync(createPost.TitleOfSection);
            if (section is null)
            {
                return PostResult.NotFound;
            }
            var post = _postFactory.Create(createPost.Title, createPost.Body, user);
            section.Posts.Add(post);
            user.IncrementPostCount();
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.Sections.Update(section);
                await _unitOfWork.Posts.CreateAsync(post);
                await _userService.ApplyRoleAsync(user);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Creating post failed");
                return PostResult.UpdateFailed;
            }
        }
        public async Task<int> GetPostsCountBySectionTitleAsync(string sectionTitle) => await _unitOfWork.Sections.GetPostsCountByTitleAsync(sectionTitle);
        public async Task<List<PostItemDto>> GetPostItemDtosByTitleAndPageAsync(GetIndexPostRequest getIndexPostRequest)
        {
            return await _cacheService.TryGetValue<GetIndexPostRequest,List<PostItemDto>>("postIndexItems", _unitOfWork.Sections.GetPostItemsWithPagingByTitleAsync, getIndexPostRequest) ?? new();
        }
        public async Task<PostResult> DeletePostByIdAsync(DeletePostRequest deletePostRequest)
        {
            var post = await _unitOfWork.Posts.GetWithUserByIdAsync(deletePostRequest.PostId);
            if (post is null)
            {
                return PostResult.NotFound;
            }
            bool shouldForbid = post.User.Id == deletePostRequest.UserId
                ? false
                : !(await _userService.IsAdminRoleByIdAsync(deletePostRequest.UserId));
            if (shouldForbid) return PostResult.Forbid;
            var repliesOfPost = await _unitOfWork.Replies.GetAllWithUserByPostIdAsync(deletePostRequest.PostId);
            var usersSet = repliesOfPost.Select(x => x.User).ToHashSet();
            var notifications = await _unitOfWork.Notifications.GetAllByReplyIdAsync(repliesOfPost.Select(x => x.Id).ToHashSet());
            foreach (var reply in repliesOfPost)
            {
                reply.User.DecrementPostCount();
            }
            post.User.DecrementPostCount();
            usersSet.Add(post.User);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (repliesOfPost.Count > 0) _unitOfWork.Replies.DeleteRange(repliesOfPost);
                if (notifications.Count > 0) _unitOfWork.Notifications.DeleteRange(notifications);
                await _userService.ApplyRoleAsync(post.User);
                _unitOfWork.Users.UpdateRange(usersSet.ToList());
                _unitOfWork.Posts.Delete(post);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Post with Id: {PostId} was not deleted", deletePostRequest.PostId);
                return PostResult.UpdateFailed;
            }
        }

    }
}
