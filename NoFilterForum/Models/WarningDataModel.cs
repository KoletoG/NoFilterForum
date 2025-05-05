using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models
{
    public class WarningDataModel
    {
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public WarningDataModel() { }
        public WarningDataModel(string content) 
        { 
            Id = Guid.NewGuid().ToString();
            Content = content; 
        }
    }
}
