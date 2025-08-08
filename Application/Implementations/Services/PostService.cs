using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web;
using Application.Interfaces.Services;
using Core.Constants;
using Core.Enums;
using Core.Implementations.Services;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Post;
using Core.Models.DTOs.InputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Post;
using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using Core.Utility;
using Ganss.Xss;
using Microsoft.AspNetCore.Identity;
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
        private readonly ICacheService _cacheService;
        public PostService(IUnitOfWork unitOfWork,ICacheService cacheService, IUserService userService, ILogger<PostService> logger, IPostFactory postFactory)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _logger = logger;
            _userService = userService;
            _postFactory = postFactory;
        }
        // GET methods
        public async Task<int> GetPostsCountBySectionTitleAsync(string sectionTitle) => await _cacheService.TryGetValue<int>($"postsCountByTitle_{sectionTitle}", _unitOfWork.Sections.GetPostsCountByTitleAsync, sectionTitle);
        public async Task<IReadOnlyCollection<PostItemDto>> GetPostItemDtosByTitleAndPageAsync(GetIndexPostRequest getIndexPostRequest) => await _cacheService.TryGetValue<GetIndexPostRequest, IReadOnlyCollection<PostItemDto>>($"postIndexItemsByTitle_{getIndexPostRequest.TitleOfSection}_ByPage_{getIndexPostRequest.Page}", _unitOfWork.Sections.GetPostItemsWithPagingByTitleAsync, getIndexPostRequest) ?? new List<PostItemDto>();  
        public async Task<string?> GetSectionTitleByPostIdAsync(string postId) => await _cacheService.TryGetValue<string?>($"titleById_{postId}", _unitOfWork.Posts.GetSectionTitleByIdAsync,postId);
        public async Task<string?> GetPostIdByReplyId(string replyId, CancellationToken cancellationToken) => await _cacheService.TryGetValue<string?>($"postIdByReplyId_{replyId}",_unitOfWork.Replies.GetPostIdByIdAsync,replyId, cancellationToken);
        public async Task<PostReplyIndexDto?> GetPostReplyIndexDtoByIdAsync(string postId, CancellationToken cancellationToken) => await _cacheService.TryGetValue<PostReplyIndexDto?>($"postReplyIndexById_{postId}", _unitOfWork.Posts.GetPostReplyIndexDtoByIdAsync,postId, cancellationToken);
        public async Task<IReadOnlyCollection<ProfilePostDto>> GetListProfilePostDtoAsync(string userId) => await _cacheService.TryGetValue<IReadOnlyCollection<ProfilePostDto>>($"profilePostDtoById_{userId}",_unitOfWork.Posts.GetListProfilePostDtoByUserIdAsync,userId) ?? [];
        public async Task<bool> HasTimeoutAsync(string userId)
        {
            var dateOfLastPost = await _unitOfWork.Posts.GetLastPostDateByUsernameAsync(userId);
            if (dateOfLastPost.AddSeconds(5) >= DateTime.UtcNow)
            {
                return !await _userService.IsAdminOrVIPAsync(userId);
            }
            return false;
        }// POST methods
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
            ReactionUtility.ApplyLikeLogic(user, post, likeDislikeRequest.PostReplyId);
            try
            {
                await _unitOfWork.RunPOSTOperationAsync(_unitOfWork.Users.Update, user, _unitOfWork.Posts.Update, post);
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
            ReactionUtility.ApplyDislikeLogic(user, post, likeDislikeRequest.PostReplyId);
            try
            {
                await _unitOfWork.RunPOSTOperationAsync(_unitOfWork.Users.Update, user, _unitOfWork.Posts.Update, post);
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Post with Id: {PostId} was not disliked", likeDislikeRequest.PostReplyId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<PostResult> PinPostAsync(string postId, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(postId);
            if (post is null)
            {
                return PostResult.NotFound;
            }
            post.TogglePin();
            try
            {
                await _unitOfWork.RunPOSTOperationAsync<PostDataModel>(_unitOfWork.Posts.Update, post, cancellationToken);
                return PostResult.Success;
            }
            catch(OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Pinning post with Id: {PostId} was canceled", postId);
                return PostResult.UpdateFailed;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Problem (un)pinning post with ID: {PostId}.", postId);
                return PostResult.UpdateFailed;
            }
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
                await _userService.ApplyRoleAsync(user); // CHANGE MAYBE
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
        public async Task<PostResult> DeletePostByIdAsync(DeletePostRequest deletePostRequest)
        {
            var post = await _unitOfWork.Posts.GetWithUserByIdAsync(deletePostRequest.PostId);
            if (post is null)
            {
                return PostResult.NotFound;
            }
            bool shouldForbid = post.User.Id == deletePostRequest.UserId
                ? false
                : !await _userService.IsAdminAsync(deletePostRequest.UserId);
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
                if (repliesOfPost.Any()) _unitOfWork.Replies.DeleteRange(repliesOfPost);
                if (notifications.Any()) _unitOfWork.Notifications.DeleteRange(notifications);
                await _userService.ApplyRoleAsync(post.User); // HERE TOO
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
