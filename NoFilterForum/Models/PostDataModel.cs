using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models
{
    public class PostDataModel
    {
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public List<ReplyDataModel> Replies { get; set; }
    }
}
