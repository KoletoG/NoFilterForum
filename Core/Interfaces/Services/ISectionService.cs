using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface ISectionService
    {
        public Task<List<SectionDataModel>> GetAllSectionsAsync();
        public Task<bool> ExistsSectionByTitleAsync(string sectionTitle);
    }
}
