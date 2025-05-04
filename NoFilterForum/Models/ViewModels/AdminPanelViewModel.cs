namespace NoFilterForum.Models.ViewModels
{
    public class AdminPanelViewModel
    {
        public List<UserDataModel> Users { get; set; }
        public AdminPanelViewModel(List<UserDataModel> users) 
        {
            Users = users;
        }
    }
}
