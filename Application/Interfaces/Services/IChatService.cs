using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.InputDTOs.Chat;
using Core.DTOs.OutputDTOs.Chat;
using Core.Enums;

namespace Application.Interfaces.Services
{
    public interface IChatService
    {
        public Task<PostResult> UpdateLastMessageAsync(UpdateLastMessageRequest request, CancellationToken cancellationToken);
        public Task<bool> ExistChatByUserIdsAsync(string userId1, string userId2, CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<IndexChatDTO>> GetIndexChatDTOsAsync(string userId,string username, CancellationToken cancellationToken);
        public Task<PostResult> CreateChat(string userId1, string userId2, CancellationToken cancellationToken);
        public Task<DetailsChatDTO?> GetChat(string id, string userId, CancellationToken cancellationToken);
    }
}
