using NoFilterForum.Repositories.Interfaces;
using NoFilterForum.Services.Interfaces;

namespace NoFilterForum.Services.Implementations
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _sectionRepository;
        public SectionService(ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }
    }
}
