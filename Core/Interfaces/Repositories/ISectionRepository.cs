using Core.Models.DTOs.InputDTOs.Post;
using Core.Models.DTOs.OutputDTOs.Post;
using Core.Models.DTOs.OutputDTOs.Section;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface ISectionRepository
    {
        public Task<SectionDataModel?> GetWithPostsByTitleAsync(string title, CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<SectionItemDto>> GetAllItemsDtoAsync(CancellationToken cancellationToken);
        public Task CreateAsync(SectionDataModel section, CancellationToken cancellationToken);
        public void Update(SectionDataModel section);
        public void Delete(SectionDataModel section); 
        public Task<int> GetPostsCountByIdAsync(string id);
        public Task<bool> ExistsSectionByTitleAsync(string sectionTitle);
        public Task<int> GetPostsCountByTitleAsync(string title);
        public Task<IReadOnlyCollection<PostItemDto>> GetPostItemsWithPagingByTitleAsync(GetIndexPostRequest getIndexPostRequest, CancellationToken cancellationToken);
        public Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken);
        public Task<SectionDataModel?> GetByIdWithPostsAndRepliesAndUsersAsync(string id, CancellationToken cancellationToken);
    }
}
