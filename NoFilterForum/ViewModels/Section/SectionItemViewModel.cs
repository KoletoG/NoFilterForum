using System.Web;

namespace Web.ViewModels.Section
{
    public class SectionItemViewModel
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public string EncodedTitle { get; set; }
        public int PostsCount { get; set; }
    }
}
