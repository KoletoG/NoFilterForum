using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFilterForum.Core.Models.DataModels;

namespace Core.Models.DataModels
{
    public class ChatDataModel
    {
        [Key]
        public string Id {  get;private set; }
        public List<MessageDataModel> MessagesUser1 { get; private set; }
        public List<MessageDataModel> MessagesUser2 { get; private set; }
        public UserDataModel User1 { get; private set; }
        public UserDataModel User2 {  get; private set; }
        public ChatDataModel()
        {
        }
        public ChatDataModel(UserDataModel user1, UserDataModel user2)
        {
            Id = Guid.NewGuid().ToString();
            User1 = user1;
            User2 = user2;
            MessagesUser1 = new List<MessageDataModel>();
            MessagesUser2 = new List<MessageDataModel>();
        }
    }
}
