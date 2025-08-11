using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.OutputDTOs.Chat;

namespace Application.Interfaces.Services
{
    public interface IChatService
    {
        public Task<IReadOnlyCollection<IndexChatDTO>> GetIndexChatDTOsAsync(string userId, CancellationToken cancellationToken);
    }
}
