using Core.Enums;

namespace Web.ViewModels
{
    public class PostIndexItemViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public UserRoles Role { get; set; }
        public string Title { get; set; }
        public bool IsPinned { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
