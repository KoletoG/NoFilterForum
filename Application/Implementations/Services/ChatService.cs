using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Core.DTOs.OutputDTOs.Chat;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Implementations.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ChatService> _logger;
        public ChatService(IUnitOfWork unitOfWork, ILogger<ChatService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<IReadOnlyCollection<IndexChatDTO>> GetIndexChatDTOsAsync(string userId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Chats.GetAll().Where(x=>x.User1.Id==userId).Select(x => new IndexChatDTO(x.Id, x.User2.UserName!, x.MessagesUser1, x.MessagesUser2)).ToListAsync(cancellationToken);
        }
        public async Task<PostResult> CreateChat(string userId1, string userId2, CancellationToken cancellationToken)
        {
            var user1 = await _unitOfWork.Users.GetByIdAsync(userId1);
            if(user1 is null)
            {
                return PostResult.NotFound;
            }
            var user2 = await _unitOfWork.Users.GetByIdAsync(userId2);
            if (user2 is null)
            {
                return PostResult.NotFound;
            }
            var chat = new ChatDataModel(user1, user2);
            try
            {
                await _unitOfWork.RunPOSTOperationAsync(_unitOfWork.Chats.Create, chat, cancellationToken);
                return PostResult.Success;
            }
            catch(OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Chat creation by user with Id: {UserId} was cancelled", userId1);
                return PostResult.UpdateFailed;
            }
            catch(DbException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "There has been a problem with the database while saving new chat by user with Id: {UserId}", userId1);
                return PostResult.UpdateFailed;
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "An error has occured creating new chat by user with Id: {UserId}", userId1);
                return PostResult.UpdateFailed;
            }
        }
    }
}
