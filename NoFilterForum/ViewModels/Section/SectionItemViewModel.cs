using System.Web;

namespace Web.ViewModels.Section
{
    public class SectionItemViewModel
    {
        public required string Title { get; set; }
        public required string Id { get; set; }
        public required string Description { get; set; }
        public required string EncodedTitle { get; set; }
        public int PostsCount { get; set; }
    }
}
