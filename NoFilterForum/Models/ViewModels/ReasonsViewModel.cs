using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Models.ViewModels
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
