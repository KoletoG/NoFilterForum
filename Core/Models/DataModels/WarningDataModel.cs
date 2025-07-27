using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Core.Models.DataModels
{
    public class WarningDataModel
    {
        [Key]
        public string Id { get; init; }
        public string Content { get; init; }
        public bool IsAccepted { get; private set; }
        public UserDataModel User { get; init; }
        public void Accept() => IsAccepted = true;
        public WarningDataModel() { }
        public WarningDataModel(string content, UserDataModel user) 
        { 
            Id = Guid.NewGuid().ToString();
            Content = content;
            User = user;
            IsAccepted = false;
        }
    }
}
