using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models
{
    public class ReplyDataModel
    {
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string Username { get; set; }
        public PostDataModel Post { get; set; }

        public ReplyDataModel(string content, string username, PostDataModel post)
        {
            Id = Guid.NewGuid().ToString();
            Content = content;
            DateCreated = DateTime.Now;
            Post = post;
            Username = username;
        }
        public ReplyDataModel()
        {

        }
    }
}
