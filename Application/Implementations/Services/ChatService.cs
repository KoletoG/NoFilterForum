using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Core.DTOs.OutputDTOs.Chat;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Implementations.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ChatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyCollection<IndexChatDTO>> GetIndexChatDTOsAsync(string userId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Chats.GetAll().Where(x=>x.User1.Id==userId).Select(x => new IndexChatDTO(x.Id, x.User2.UserName!, x.MessagesUser1, x.MessagesUser2)).ToListAsync(cancellationToken);
        }
    }
}
