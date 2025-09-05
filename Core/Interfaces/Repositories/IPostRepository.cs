using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IPostRepository
    {
        public IQueryable<PostDataModel> GetAll();
        public Task<PostDataModel?> GetByIdAsync(string id);
        public Task<PostDataModel?> GetWithRepliesByIdAsync(string id, CancellationToken cancellationToken);
        public Task<PostDataModel?> GetWithUserByIdAsync(string id, CancellationToken cancellationToken);
        public Task<string?> GetSectionTitleByIdAsync(string postId,CancellationToken cancellationToken);
        public Task<PostReplyIndexDto?> GetPostReplyIndexDtoByIdAsync(string id, CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<PostDataModel>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken);
        public Task CreateAsync(PostDataModel post, CancellationToken cancellationToken);
        public void Update(PostDataModel post);
        public void UpdateRange(IEnumerable<PostDataModel> posts);
        public Task<IDictionary<string, ProfilePostDto>> GetListProfilePostDtoByUserIdAsync(string username, CancellationToken cancellationToken);
        public void Delete(PostDataModel post);
        public void DeleteRange(IEnumerable<PostDataModel> posts);
        public Task<DateTime> GetLastPostDateByUsernameAsync(string userId, CancellationToken cancellationToken);
        public Task<bool> ExistByIdAsync(string id, CancellationToken cancellationToken);
    }
}
