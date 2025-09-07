using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DataModels
{
    public class MessageDataModel
    {
        [Key]
        public string Id { get;private set; }
        public DateTime DateTime { get; private set; }
        public string Message { get; private set; }
        public string UserId { get; private set; }
        public ChatDataModel Chat { get; private set; }
        public string ChatId { get; private set; }
        public MessageDataModel() { }
        public MessageDataModel(string message, string userId, ChatDataModel chat)
        {
            DateTime = DateTime.UtcNow;
            Message = message;
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            Chat = chat;
            ChatId = chat.Id;
        }
    }
}
