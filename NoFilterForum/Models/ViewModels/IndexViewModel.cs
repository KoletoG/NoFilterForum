using Microsoft.AspNetCore.Components.Web;
using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Models.ViewModels
{
    public class IndexViewModel
    {
        public List<SectionDataModel> Sections { get; set; }
        public bool IsAdmin { get; set; }
        public IndexViewModel(List<SectionDataModel> sections,bool isAdmin) 
        {
            Sections = sections;
            IsAdmin = isAdmin;
        }
    }
}
