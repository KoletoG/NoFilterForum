using System.ComponentModel.DataAnnotations;
using Core.Constants;
using Core.Interfaces.Business_Logic;

namespace NoFilterForum.Core.Models.DataModels
{
    public class ReplyDataModel : ILikeDislike
    {
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public PostDataModel Post { get; set; }
        public UserDataModel User { get; set; }
        public short Likes { get; set; }
        public string PostId { get; set; }
        public void SetDefaultUser() => User = UserConstants.DefaultUser;
        public void IncrementLikes() => Likes++;
        public void DecrementLikes() => Likes--;
        public ReplyDataModel(string content, UserDataModel user, PostDataModel post)
        {
            Id = Guid.NewGuid().ToString();
            Content = content;
            DateCreated = DateTime.UtcNow;
            Post = post;
            User = user;
            Likes = 0;
            PostId = post.Id;
        }
        public ReplyDataModel()
        {

        }
    }
}
