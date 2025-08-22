using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.InputDTOs.Message;
using Core.DTOs.OutputDTOs.Message;
using Core.Enums;

namespace Application.Interfaces.Services
{
    public interface IMessageService
    {
        public Task<PostResult> DeleteAsync(string messageId, string userId, string chatId);
        public Task<CreateMessageDTO> CreateMessageAsync(CreateMessageRequest createMessageRequest, CancellationToken cancellationToken);
    }
}
