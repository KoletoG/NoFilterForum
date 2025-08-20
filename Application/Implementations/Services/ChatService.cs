using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Core.DTOs.InputDTOs.Chat;
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
        public async Task<bool> ExistChatByUserIdsAsync(string userId1, string userId2,CancellationToken cancellationToken)
        {
            return await _unitOfWork.Chats.GetAll().AnyAsync(x => ((x.User1.Id == userId1 && x.User2.Id==userId2) || (x.User1.Id==userId2 && x.User2.Id==userId1)),cancellationToken);
        }
        public async Task<IReadOnlyCollection<IndexChatDTO>> GetIndexChatDTOsAsync(string userId,string username, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Chats.GetAll().Where(x=>x.User1.Id==userId || x.User2.Id==userId).Select(x => new IndexChatDTO(x.Id, x.User1.UserName! == username ? x.User2.UserName! : x.User1.UserName!, x.Messages,userId == x.User1.Id ? x.User2.Role : x.User1.Role)).ToListAsync(cancellationToken);
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
            // Below line checks if user1 and user2 are the same and also checks if they already have a chat together
            if(await _unitOfWork.Chats.GetAll().AnyAsync(x => (x.User1 == user1 || x.User1 == user2) && (x.User2 == user1 || x.User2 == user2)))
            {
                return PostResult.Conflict;
            }
            var chat = new ChatDataModel(user1, user2);
            try
            {
                await _unitOfWork.RunPOSTOperationAsync(_unitOfWork.Chats.CreateAsync, chat, cancellationToken);
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
        public async Task<string?> GetIdOfLastMessageAsync(string userId, string chatId)
        {
            // Add validation
            return await _unitOfWork.Chats.GetAll()
                .Where(x => x.Id == chatId)
                .Select(x => x.User1.Id == userId ? x.LastMessageSeenByUser2 : x.LastMessageSeenByUser1) // 
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }
        public async Task<PostResult> UpdateLastMessageAsync(UpdateLastMessageRequest request, CancellationToken cancellationToken)
        {
            var chat = await _unitOfWork.Chats.GetAll().Include(x=>x.User1).Include(x=>x.User2).Where(x => x.Id == request.ChatId).FirstOrDefaultAsync(cancellationToken);
            if (chat is null) return PostResult.NotFound;
            if (chat.User1.Id != request.UserId && chat.User2.Id != request.UserId) return PostResult.Forbid;
            var message = await _unitOfWork.Message.GetByIdAsync(request.MessageId);
            if(message is null) return PostResult.NotFound;
            if (chat.User1.Id == request.UserId)
            {
                chat.ChangeLastSeenByUser2(message);
            }
            else
            {
                chat.ChangeLastSeenByUser1(message);
            }
            try
            {
                await _unitOfWork.RunPOSTOperationAsync(_unitOfWork.Chats.Update, chat,cancellationToken);
                return PostResult.Success;
            }
            catch(OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Updating last message of chat with Id: {ChatId} was cancelled", request.ChatId);
                return PostResult.UpdateFailed;
            }
            catch (DbException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "There was a problem with the database updating last message of chat with Id: {ChatId}", request.ChatId);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<DetailsChatDTO?> GetChat(string id, string userId, CancellationToken cancellationToken)
        {
            try
            {
                var chat = await _unitOfWork.Chats.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new DetailsChatDTO(
                    x.User1.UserName!,
                    x.User2.UserName!,
                    x.User1.Id,
                    x.User2.Id,
                    x.Messages.OrderByDescending(x=>x.DateTime).Take(100).ToList(),
                    x.Id)
                ).FirstOrDefaultAsync(cancellationToken);
                if (chat is null)
                {
                    return null;
                }
                return chat;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex,"Getting chat information by user with Id: {UserId} was cancelled", userId);
                return null;
            }
        }
    }
}
