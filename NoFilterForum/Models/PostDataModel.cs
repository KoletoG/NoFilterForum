using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoFilterForum.Models
{
    public class PostDataModel
    {
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public PostDataModel(string title, string content, string userName)
        {
            Id = Guid.NewGuid().ToString();
            Content = content;
            Title = title;
            DateCreated = DateTime.Now;
            Username = userName;
        }
        public PostDataModel()
        {

        }
    }
}
