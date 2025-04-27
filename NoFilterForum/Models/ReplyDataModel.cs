using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models
{
    public class ReplyDataModel
    {
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public UserDataModel User { get; set; }
        public PostDataModel Post { get; set; }

        public ReplyDataModel(string content, UserDataModel user, PostDataModel post)
        {
            Id = Guid.NewGuid().ToString();
            Content = content;
            DateCreated = DateTime.Now;
            User = user;
            Post = post;
        }
        public ReplyDataModel()
        {

        }
    }
}
