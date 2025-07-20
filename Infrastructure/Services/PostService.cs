using System.Web;
using Core.Constants;
using Core.Enums;
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
        private readonly IHtmlSanitizer _htmlSanitizer;
        public PostService(IUnitOfWork unitOfWork,IUserService userService ,ILogger<PostService> logger,IPostFactory postFactory, IHtmlSanitizer htmlSanitizer)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userService = userService;
            _postFactory = postFactory;
            _htmlSanitizer = htmlSanitizer;
            _htmlSanitizer.AllowedTags.Clear();
            _htmlSanitizer.AllowedTags.Add("a");
        }
        public async Task<PostResult> LikeAsync(LikeDislikeRequest likeDislikeRequest)
        {
            var user = await _userService.GetUserByIdAsync(likeDislikeRequest.UserId);
            if (user == null)
            {
                return PostResult.NotFound;
            }
            var post = await _unitOfWork.Posts.GetByIdAsync(likeDislikeRequest.PostReplyId);
            if (post == null)
            {
                return PostResult.NotFound;
            }
            var wasLiked = user.LikesPostRepliesIds.Contains(likeDislikeRequest.PostReplyId);
            var wasDisliked = user.DislikesPostRepliesIds.Contains(likeDislikeRequest.PostReplyId);
            if (wasDisliked)
            {
                post.IncrementLikes();
                user.DislikesPostRepliesIds.Remove(likeDislikeRequest.PostReplyId);
            }
            if (wasLiked)
            {
                post.DecrementLikes();
                user.LikesPostRepliesIds.Remove(likeDislikeRequest.PostReplyId);
            }
            else
            {
                post.IncrementLikes();
                user.LikesPostRepliesIds.Add(likeDislikeRequest.PostReplyId);
            }
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
            if (user == null)
            {
                return PostResult.NotFound;
            }
            var post = await _unitOfWork.Posts.GetByIdAsync(likeDislikeRequest.PostReplyId);
            if (post == null)
            {
                return PostResult.NotFound;
            }
            var wasLiked = user.LikesPostRepliesIds.Contains(likeDislikeRequest.PostReplyId);
            var wasDisliked = user.DislikesPostRepliesIds.Contains(likeDislikeRequest.PostReplyId);
            if (wasLiked)
            {
                post.DecrementLikes();
                user.LikesPostRepliesIds.Remove(likeDislikeRequest.PostReplyId);
            }
            if (wasDisliked)
            {
                post.IncrementLikes();
                user.DislikesPostRepliesIds.Remove(likeDislikeRequest.PostReplyId);
            }
            else
            {
                post.DecrementLikes();
                user.DislikesPostRepliesIds.Add(likeDislikeRequest.PostReplyId);
            }
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
        public async Task<string> GetSectionTitleByPostIdAsync(string postId)
        {
            return await _unitOfWork.Posts.GetSectionTitleByIdAsync(postId);
        }
        public async Task<string> GetPostIdByReplyId(string replyId)
        {
            return await _unitOfWork.Replies.GetPostIdById(replyId);
        }
        public async Task<PostReplyIndexDto> GetPostReplyIndexDtoByIdAsync(string postId)
        {
            return await _unitOfWork.Posts.GetPostReplyIndexDtoByIdAsync(postId);
        }
        public async Task<List<ProfilePostDto>> GetListProfilePostDtoAsync(GetProfilePostDtoRequest getProfilePostDtoRequest)
        {
            return await _unitOfWork.Posts.GetListProfilePostDtoByUserIdAsync(getProfilePostDtoRequest.UserId);
        }
        public async Task<PostResult> PinPostAsync(string postId)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(postId);
            if (post == null)
            {
                return PostResult.NotFound;
            }
            post.TogglePin();
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.Posts.Update(post);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError($"Problem (un)pinning post with ID: {postId}.");
                return PostResult.UpdateFailed;
            }
        }
        public async Task<bool> HasTimeoutAsync(string userId)
        {
            var dateOfLastPost = await _unitOfWork.Posts.GetLastPostDateByUsernameAsync(userId);
            if (dateOfLastPost == default)
            {
                return false;
            }
            if (dateOfLastPost.AddMinutes(PostConstants.TimeoutPosts) > DateTime.Now)
            {
                return true;
            }
            return false;
        }
        public async Task<PostResult> CreatePostAsync(CreatePostRequest createPost)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(createPost.UserId);
            if (user == null)
            {
                return PostResult.NotFound;
            }
            var section = await _unitOfWork.Sections.GetWithPostsByTitleAsync(createPost.TitleOfSection);
            if (section == null)
            {
                return PostResult.NotFound;
            }
            var post = _postFactory.Create(createPost.Title, createPost.Body, user);
            section.Posts.Add(post);
            user.IncrementPostCount();
            RoleUtility.AdjustRoleByPostCount(user);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.Sections.Update(section);
                await _unitOfWork.Posts.CreateAsync(post);
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
        public async Task<int> GetPostsCountBySectionTitleAsync(string sectionTitle)
        {
            return await _unitOfWork.Sections.GetPostsCountByTitleAsync(sectionTitle);
        }
        public async Task<List<PostItemDto>> GetPostItemDtosByTitleAndPageAsync(GetIndexPostRequest getIndexPostRequest)
        {
            return await _unitOfWork.Sections.GetPostItemsWithPagingByTitleAsync(getIndexPostRequest.TitleOfSection, getIndexPostRequest.Page, PostConstants.PostsPerSection);
        }
        public async Task<PostResult> DeletePostByIdAsync(DeletePostRequest deletePostRequest)
        {
            var post = await _unitOfWork.Posts.GetWithUserByIdAsync(deletePostRequest.PostId);
            if (post == null)
            {
                return PostResult.NotFound;
            }
            if (post.User.Id != deletePostRequest.UserId)
            {
                if(!await _userService.IsAdminRoleByIdAsync(deletePostRequest.UserId))
                {
                    return PostResult.Forbid;
                }
            }
            var repliesOfPost = await _unitOfWork.Replies.GetAllWithUserByPostIdAsync(deletePostRequest.PostId);
            HashSet<UserDataModel> users = new HashSet<UserDataModel>();
            var notifications = new List<NotificationDataModel>();
            foreach(var reply in repliesOfPost)
            {
                notifications.AddRange(await _unitOfWork.Notifications.GetAllByReplyIdAsync(reply.Id));
                reply.User.DecrementPostCount();
                users.Add(reply.User);
            }
            post.User.DecrementPostCount();
            users.Add(post.User);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.Replies.DeleteRange(repliesOfPost);
                _unitOfWork.Notifications.DeleteRange(notifications);
                _unitOfWork.Users.UpdateRange(users.ToList());
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
