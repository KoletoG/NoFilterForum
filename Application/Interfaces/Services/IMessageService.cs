using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.InputDTOs.Message;
using Core.Enums;

namespace Application.Interfaces.Services
{
    public interface IMessageService
    {
        public Task<PostResult> CreateMessageAsync(CreateMessageRequest createMessageRequest, CancellationToken cancellationToken);
    }
}
