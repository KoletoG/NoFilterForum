using NoFilterForum.Core.Models.DataModels;

namespace Web.ViewModels.Admin
{
    public class ReasonsViewModel
    {
        public required IEnumerable<UserReasonViewModel> Users { get; set; }
    }
}
