using Core.Enums;

namespace Web.ViewModels
{
    public class PostIndexItemViewModel
    {
        public string PostId { get; set; }
        public string Username { get; set; }
        public UserRoles Role { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
