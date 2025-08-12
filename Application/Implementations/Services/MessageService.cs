using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Core.DTOs.InputDTOs.Message;
using Core.DTOs.OutputDTOs.Message;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Implementations.Services
{
    public class MessageService(IUnitOfWork unitOfWork, ILogger<MessageService> logger, IMessageFactory messageFactory) : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<MessageService> _logger = logger;
        private readonly IMessageFactory _messageFactory = messageFactory;
        public async Task<CreateMessageDTO> CreateMessageAsync(CreateMessageRequest createMessageRequest, CancellationToken cancellationToken)
        {
            var chat = await _unitOfWork.Chats.GetAll()
                .Include(x => x.User1)
                .Include(x => x.User2)
                .Include(x=>x.Messages)
                .FirstOrDefaultAsync(x => x.Id == createMessageRequest.ChatId,cancellationToken);
            if(chat is null)
            {
                return new(PostResult.NotFound,null);
            }
            // If the person is not from this chat, forbid
            if (chat.User1.Id != createMessageRequest.UserId && chat.User2.Id != createMessageRequest.UserId)
            {
                return new(PostResult.Forbid,null);
            }
            var message = _messageFactory.Create(createMessageRequest.Message, createMessageRequest.UserId);
            chat.Messages.Add(message);
            try
            {
                await _unitOfWork.RunPOSTOperationAsync(_unitOfWork.Message.CreateAsync, message,_unitOfWork.Chats.Update,chat, cancellationToken);
                return new(PostResult.Success,message.Message);
            }
            catch(OperationCanceledException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Creation of message by user with Id: {UserId} was cancelled", createMessageRequest.UserId);
                return new(PostResult.UpdateFailed, null);
            }
            catch(DbException ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "There was a problem with the database saving new message by user with Id: {UserId}", createMessageRequest.UserId);
                return new(PostResult.UpdateFailed, null);
            }
        }
    }
}
