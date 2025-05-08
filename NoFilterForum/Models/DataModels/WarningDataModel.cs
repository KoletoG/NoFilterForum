using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models.DataModels
{
    public class WarningDataModel
    {
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public bool IsAccepted { get; set; }
        public UserDataModel User { get; set; }
        public WarningDataModel() { }
        public WarningDataModel(string content, UserDataModel user) 
        { 
            Id = Guid.NewGuid().ToString();
            Content = content;
            User = user;
        }
    }
}
