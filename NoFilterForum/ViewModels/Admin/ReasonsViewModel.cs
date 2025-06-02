using NoFilterForum.Core.Models.DataModels;

namespace Web.ViewModels.Admin
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
