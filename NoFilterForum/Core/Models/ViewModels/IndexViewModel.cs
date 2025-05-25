using Microsoft.AspNetCore.Components.Web;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Models.ViewModels
{
    public class IndexViewModel
    {
        public List<SectionDataModel> Sections { get; set; }
        public bool IsAdmin { get; set; }
        public SectionDataModel Section { get; set; }
        public IndexViewModel(List<SectionDataModel> sections,bool isAdmin) 
        {
            Sections = sections;
            IsAdmin = isAdmin;
            Section = new SectionDataModel();
        }
    }
}
