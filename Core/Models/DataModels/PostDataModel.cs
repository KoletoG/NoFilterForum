using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Constants;

namespace NoFilterForum.Core.Models.DataModels
{
    public class PostDataModel
    {
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string Title { get; set; }
        public UserDataModel User { get; set; }
        public short Likes { get; set; }
        public List<ReplyDataModel> Replies { get; set; }
        public bool IsPinned { get; set; }
        public void TogglePin() => IsPinned = !IsPinned;
        public void SetDefaultUser() => User = UserConstants.DefaultUser;
        public PostDataModel(string title, string content, UserDataModel user)
        {
            Id = Guid.NewGuid().ToString();
            Content = content;
            Title = title;
            DateCreated = DateTime.UtcNow;
            User = user;
            Replies = new List<ReplyDataModel>();
            Likes = 0;
            IsPinned = false;
        }
        public PostDataModel()
        {

        }
    }
}
