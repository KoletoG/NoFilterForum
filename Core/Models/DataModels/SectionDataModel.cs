using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Core.Models.DataModels
{
    public class SectionDataModel
    {
        [Key]
        public string Id { get; init; }
        public ICollection<PostDataModel> Posts { get; init; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="Title cannot be empty.")]
        public string Title { get; init; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title cannot be empty.")]
        [MinLength(10,ErrorMessage ="Description should be at least 10 characters long.")]
        public string Description { get; init; }
        public SectionDataModel()
        {

        }
        public SectionDataModel(string title, string description)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            Posts = new List<PostDataModel>();
        }
    }
}
