using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Services.Interfaces
{
    public interface ISectionService
    {
        public Task<List<SectionDataModel>> GetAllSectionsAsync();
    }
}
