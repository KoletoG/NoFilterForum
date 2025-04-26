using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models
{
    public class ReplyDataModel
    {
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }

        public ReplyDataModel(string content)
        {
            Id = Guid.NewGuid().ToString();
            Content = content;
            DateCreated = DateTime.Now;
        }
    }
}
