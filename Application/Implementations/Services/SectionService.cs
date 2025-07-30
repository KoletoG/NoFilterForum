using Core.Constants;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Section;
using Core.Models.DTOs.OutputDTOs.Section;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class SectionService : ISectionService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ILogger<SectionService> _logger;
        private readonly ISectionFactory _sectionFactory;
        public SectionService(IUnitOfWork unitOfWork, IUserService userService, ISectionFactory sectionFactory, IMemoryCache memoryCache, ILogger<SectionService> logger)
        {
            _logger = logger;
            _sectionFactory = sectionFactory;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }
        public async Task<List<SectionItemDto>> GetAllSectionItemDtosAsync() // Move caching to another class
        {
            if (!_memoryCache.TryGetValue("sections", out List<SectionItemDto> sections))
            {
                sections = await _unitOfWork.Sections.GetAllItemsDtoAsync();
                MemoryCacheEntryOptions memoryCacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                _memoryCache.Set("sections", sections, memoryCacheOptions);
            }
            return sections;
        }
        public async Task<bool> ExistsSectionByTitleAsync(string sectionTitle)
        {
            if (string.IsNullOrEmpty(sectionTitle))
            {
                return false;
            }
            return await _unitOfWork.Sections.ExistsByTitleAsync(sectionTitle);
        }
        public async Task<int> GetPostsCountByIdAsync(string sectionId) => await _unitOfWork.Sections.GetPostsCountByIdAsync(sectionId);
        public async Task<PostResult> CreateSectionAsync(CreateSectionRequest createSectionRequest)
        {
            if (!await _userService.IsAdminRoleByIdAsync(createSectionRequest.UserId))
            {
                return PostResult.Forbid;
            }
            var section = _sectionFactory.Create(createSectionRequest.Title, createSectionRequest.Description);
            try
            {
                await _unitOfWork.RunPOSTOperationAsync<SectionDataModel>(_unitOfWork.Sections.CreateAsync, section);
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Section with title: {SectionTitle} wasn't created", createSectionRequest.Title);
                return PostResult.UpdateFailed;
            }
        }
        private (HashSet<UserDataModel> users, List<ReplyDataModel> replies) ProcessPosts(List<PostDataModel> posts)
        {
            var users = new HashSet<UserDataModel>();
            var replies = posts.SelectMany(x => x.Replies).ToList();
            foreach (var post in posts)
            {
                DecrementAndCollect(users, post.User);
            }
            foreach (var reply in replies)
            {
                DecrementAndCollect(users, reply.User);
            }
            return (users, replies);
        }
        private void DecrementAndCollect(HashSet<UserDataModel> users, UserDataModel user)
        {
            if (user != UserConstants.DefaultUser)
            {
                user.DecrementPostCount();
                users.Add(user);
            }
        }
        public async Task<PostResult> DeleteSectionAsync(DeleteSectionRequest deleteSectionRequest)
        {
            var section = await _unitOfWork.Sections.GetByIdWithPostsAndRepliesAndUsersAsync(deleteSectionRequest.SectionId);
            if (section is null)
            {
                return PostResult.NotFound;
            }
            var posts = section.Posts;
            (var usersSet,var replies) = ProcessPosts(posts);
            var notifications = new List<NotificationDataModel>();
            foreach(var reply in replies)
            {
                notifications.AddRange(await _unitOfWork.Notifications.GetAllByReplyIdAsync(reply.Id));
            }
            var users = usersSet.ToList();
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.Users.UpdateRange(users);
                _unitOfWork.Posts.DeleteRange(posts);
                _unitOfWork.Replies.DeleteRange(replies);
                _unitOfWork.Sections.Delete(section);
                _unitOfWork.Notifications.DeleteRange(notifications);
                foreach(var user in users)
                {
                    await _userService.ApplyRoleAsync(user);
                }
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                _memoryCache.Remove("sections");
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Section with Id: {SectionId} wasn't deleted", deleteSectionRequest.SectionId);
                return PostResult.UpdateFailed;
            }
        }
    }
}
