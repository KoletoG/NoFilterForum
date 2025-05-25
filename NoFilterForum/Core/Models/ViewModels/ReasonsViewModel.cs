using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Models.ViewModels
{
    public class ReasonsViewModel
    {
        public List<UserDataModel> Users { get; set; }
        public ReasonsViewModel(List<UserDataModel> users) 
        {
            Users = users;
        }
    }
}
