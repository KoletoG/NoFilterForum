using Core.Enums;
using Core.Models.DTOs.OutputDTOs;
using Web.ViewModels.Post;
using Web.ViewModels.Reply;

namespace Web.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public ProfileUserDto Profile { get; set; }
        public bool IsSameUser { get; set; }
        public List<ReplyItemViewModel> Replies { get; set; }
        public List<PostItemViewModel> Posts { get; set; }
        public int Page {  get; set; }
        public int TotalPages { get; set; }
        public Dictionary<string, DateTime> UserIdDate { get; set; }
    }
}
