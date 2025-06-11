using Core.Models.DTOs.OutputDTOs.Post;
using Core.Models.DTOs.OutputDTOs.Section;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface ISectionRepository
    {
        public Task<SectionDataModel> GetByIdAsync(string id);
        public Task<SectionDataModel> GetWithPostsByTitleAsync(string title);
        public Task<List<SectionDataModel>> GetAllAsync();
        public Task<List<SectionItemDto>> GetAllItemsDtoAsync();
        public Task<SectionDataModel> CreateAsync(SectionDataModel section);
        public Task UpdateAsync(SectionDataModel section);
        public Task DeleteAsync(SectionDataModel section);
        public Task<bool> ExistsSectionByTitleAsync(string sectionTitle);
        public Task<int> GetPostsCountByTitleAsync(string title);
        public Task<List<PostItemDto>> GetPostItemsWithPagingByTitleAsync(string sectionTitle, int page, int countPerPage);
        public Task<bool> ExistsByTitleAsync(string title);
        public Task<SectionDataModel> GetByIdWithPostsAndRepliesAndUsersAsync(string id);
    }
}
