using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface ISectionRepository
    {
        public Task<SectionDataModel> GetByIdAsync(string id);
        public Task<SectionDataModel> GetByTitleAsync(string title);
        public Task<List<SectionDataModel>> GetAllAsync();
        public Task<SectionDataModel> CreateAsync(SectionDataModel section);
        public Task UpdateAsync(SectionDataModel section);
        public Task DeleteAsync(SectionDataModel section);
        public Task<bool> ExistsByTitleAsync(string title);
    }
}
