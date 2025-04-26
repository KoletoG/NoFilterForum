using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models
{
    public class ReplyDataModel
    {
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }

        public ReplyDataModel(string content, string userId)
        {
            Id = Guid.NewGuid().ToString();
            Content = content;
            DateCreated = DateTime.Now;
            UserId = userId;
        }
    }
}
