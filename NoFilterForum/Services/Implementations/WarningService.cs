using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;

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
