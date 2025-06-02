using System.Web;
using Core.Constants;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using Core.Utility;
using Ganss.Xss;
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
        private readonly ILogger<PostService> _logger;
        private readonly IPostFactory _postFactory;
        private readonly IHtmlSanitizer _htmlSanitizer;
        public PostService(IUnitOfWork unitOfWork, ILogger<PostService> logger,IPostFactory postFactory, IHtmlSanitizer htmlSanitizer)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _postFactory = postFactory;
            _htmlSanitizer = htmlSanitizer;
            _htmlSanitizer.AllowedTags.Clear();
            _htmlSanitizer.AllowedTags.Add("a");
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
                await _unitOfWork.Posts.UpdateAsync(post);
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
                await _unitOfWork.Sections.UpdateAsync(section);
                await _unitOfWork.Posts.CreateAsync(post);
                await _unitOfWork.Users.UpdateAsync(user);
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
    }
}
