using Core.Models.DTOs.InputDTOs.Post;
using Core.Models.DTOs.OutputDTOs.Post;
using Core.Models.DTOs.OutputDTOs.Section;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface ISectionRepository
    {
        public Task<SectionDataModel?> GetByIdAsync(string id);
        public Task<SectionDataModel?> GetWithPostsByTitleAsync(string title);
        public Task<IReadOnlyCollection<SectionDataModel>> GetAllAsync();
        public Task<IReadOnlyCollection<SectionItemDto>> GetAllItemsDtoAsync();
        public Task<SectionDataModel> CreateAsync(SectionDataModel section);
        public void Update(SectionDataModel section);
        public void Delete(SectionDataModel section);
        public Task<int> GetPostsCountByIdAsync(string id);
        public Task<bool> ExistsSectionByTitleAsync(string sectionTitle);
        public Task<int> GetPostsCountByTitleAsync(string title);
        public Task<IReadOnlyCollection<PostItemDto>> GetPostItemsWithPagingByTitleAsync(GetIndexPostRequest getIndexPostRequest);
        public Task<bool> ExistsByTitleAsync(string title);
        public Task<SectionDataModel?> GetByIdWithPostsAndRepliesAndUsersAsync(string id);
    }
}
