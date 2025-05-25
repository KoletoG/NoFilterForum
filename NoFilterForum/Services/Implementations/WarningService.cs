using NoFilterForum.Repositories.Interfaces;
using NoFilterForum.Services.Interfaces;

namespace NoFilterForum.Services.Implementations
{
    public class WarningService : IWarningService
    {
        private readonly IWarningRepository _warningRepository;
        public WarningService(IWarningRepository warningRepository)
        {
            _warningRepository = warningRepository;
        }
    }
}
