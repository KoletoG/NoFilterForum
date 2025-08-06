using System.Runtime.CompilerServices;
using Application.Interfaces.Services;
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
using NuGet.Packaging;

namespace Application.Implementations.Services
{
    public class SectionService(IUnitOfWork unitOfWork, IUserService userService, ISectionFactory sectionFactory, ILogger<SectionService> logger,ICacheService cacheService) : ISectionService
    {
        private readonly ICacheService _cacheService = cacheService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IUserService _userService = userService;
        private readonly ILogger<SectionService> _logger = logger;
        private readonly ISectionFactory _sectionFactory = sectionFactory;
        public async Task<IReadOnlyCollection<SectionItemDto>> GetAllSectionItemDtosAsync()
        {
            var sections = await _cacheService.TryGetValue("listSectionItemDto", _unitOfWork.Sections.GetAllItemsDtoAsync);
            return sections ?? [];
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
            if (!await _userService.IsAdminAsync(createSectionRequest.UserId))
            {
                return PostResult.Forbid;
            }
            var section = _sectionFactory.Create(createSectionRequest.Title, createSectionRequest.Description);
            try
            {
                await _unitOfWork.RunPOSTOperationAsync(_unitOfWork.Sections.CreateAsync, section);
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Section with title: {SectionTitle} wasn't created", createSectionRequest.Title);
                return PostResult.UpdateFailed;
            }
        }
        private static (HashSet<UserDataModel> users, List<ReplyDataModel> replies) ProcessPosts(IReadOnlyCollection<PostDataModel> posts)
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
        private static void DecrementAndCollect(HashSet<UserDataModel> users, UserDataModel user)
        {
            if (user != UserConstants.DefaultUser)
            {
                user.DecrementPostCount();
                users.Add(user);
            }
        }
        public async Task<PostResult> DeleteSectionAsync(DeleteSectionRequest deleteSectionRequest)
        {
            if(!await _userService.IsAdminAsync(deleteSectionRequest.UserId))
            {
                return PostResult.Forbid;
            }
            var section = await _unitOfWork.Sections.GetByIdWithPostsAndRepliesAndUsersAsync(deleteSectionRequest.SectionId);
            if (section is null)
            {
                return PostResult.NotFound;
            }
            var posts = section.Posts;
            (var usersSet,var replies) = ProcessPosts(posts);
            var notifications = await _unitOfWork.Notifications.GetAllByReplyIdsAsync(replies.Select(x=>x.Id));
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.Users.UpdateRange(usersSet);
                _unitOfWork.Posts.DeleteRange(posts);
                _unitOfWork.Replies.DeleteRange(replies);
                _unitOfWork.Sections.Delete(section);
                _unitOfWork.Notifications.DeleteRange(notifications);
                foreach (var user in usersSet)
                {
                    await _userService.ApplyRoleAsync(user); // do something here
                }
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
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
