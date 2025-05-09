using System.ComponentModel.DataAnnotations;
using static System.Collections.Specialized.BitVector32;

namespace NoFilterForum.Models.DataModels
{
    public class SectionDataModel
    {
        [Key]
        public string Id { get; set; }
        public List<PostDataModel> Posts { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="Title cannot be empty.")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title cannot be empty.")]
        [MinLength(10,ErrorMessage ="Description should be at least 10 characters long.")]
        public string Description { get; set; }
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
