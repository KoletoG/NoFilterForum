using Core.Enums;

namespace Web.ViewModels.Post
{
    public class PostIndexViewModel
    {
        public required IEnumerable<PostIndexItemViewModel> Posts { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public required string TitleOfSection { get; set; }
    }
}
