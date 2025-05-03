using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models
{
    public class SectionDataModel
    {
        [Key]
        public string Id { get; set; }
        public List<PostDataModel> Posts { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public SectionDataModel() { }
        public SectionDataModel(string title, string description)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            Posts = new List<PostDataModel>();
        }
    }
}
