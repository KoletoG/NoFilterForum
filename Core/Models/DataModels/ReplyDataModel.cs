using System.ComponentModel.DataAnnotations;
using Core.Constants;

namespace NoFilterForum.Core.Models.DataModels
{
    public class ReplyDataModel
    {
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public PostDataModel Post { get; set; }
        public UserDataModel User { get; set; }
        public short Likes { get; set; }
        public void SetDefaultUser() => User = UserConstants.DefaultUser;
        public ReplyDataModel(string content, UserDataModel user, PostDataModel post)
        {
            Id = Guid.NewGuid().ToString();
            Content = content;
            DateCreated = DateTime.UtcNow;
            Post = post;
            User = user;
            Likes = 0;
        }
        public ReplyDataModel()
        {

        }
    }
}
