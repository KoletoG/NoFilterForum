using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models
{
    public class PostDataModel
    {
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public List<ReplyDataModel> Replies { get; set; }
        public DateTime DateCreated { get; set; }

        public PostDataModel(string content)
        {
            Id = Guid.NewGuid().ToString();
            Content = content;
            Replies = new List<ReplyDataModel>();
            DateCreated = DateTime.Now;
        }
    }
}
