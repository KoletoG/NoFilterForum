using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models
{
    public class ReportDataModel
    {
        [Key]
        public string Id { get; set; }
        public UserDataModel User { get; set; }
        public string Content { get; set; }
        public ReportDataModel() { }
        public ReportDataModel(UserDataModel user, string content)
        {
            Id = Guid.NewGuid().ToString();
            User = user;
            Content = content;
        }
    }
}
