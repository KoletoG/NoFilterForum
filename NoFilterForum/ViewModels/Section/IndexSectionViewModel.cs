using NoFilterForum.Core.Models.ViewModels;

namespace Web.ViewModels.Section
{
    public class IndexSectionViewModel
    {
        public bool IsAdmin { get; set; }
        public List<SectionItemViewModel> Sections { get; set; }
        public CreateSectionViewModel CreateSection { get; set; }
    }
}
