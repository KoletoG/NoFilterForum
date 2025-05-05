using Microsoft.AspNetCore.Components.Web;

namespace NoFilterForum.Models.ViewModels
{
    public class IndexViewModel
    {
        public List<SectionDataModel> Sections { get; set; }
        public List<WarningDataModel> Warnings { get; set; }
        public bool IsAdmin { get; set; }
        public IndexViewModel(List<SectionDataModel> sections,bool isAdmin, List<WarningDataModel> warnings) 
        {
            Sections = sections;
            IsAdmin = isAdmin;
            Warnings = warnings;
        }
    }
}
