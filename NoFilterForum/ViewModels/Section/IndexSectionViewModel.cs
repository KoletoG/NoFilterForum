using NoFilterForum.Core.Models.ViewModels;

namespace Web.ViewModels.Section
{
    public class IndexSectionViewModel
    {
        public bool IsAdmin { get; set; }
        public required IEnumerable<SectionItemViewModel> Sections { get; set; }
    }
}
