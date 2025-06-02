using Core.Enums;

namespace Web.ViewModels.Post
{
    public class PostIndexViewModel
    {
        public List<PostIndexItemViewModel> Posts { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public string TitleOfSection { get; set; }
    }
}
