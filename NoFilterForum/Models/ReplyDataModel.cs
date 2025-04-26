using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models
{
    public class ReplyDataModel
    {
        [Key]
        public string Id { get; set; }
        public PostDataModel ForPost { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
