using Core.Enums;
using Core.Models.DTOs.OutputDTOs;
using Web.ViewModels.Post;
using Web.ViewModels.Reply;

namespace Web.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public required ProfileUserViewModel Profile { get; set; }
        public bool IsSameUser { get; set; }
        public required IDictionary<string,ReplyItemViewModel> Replies { get; set; }
        public required IDictionary<string, PostItemViewModel> Posts { get; set; }
        public int Page {  get; set; }
        public int TotalPages { get; set; }
        public required IDictionary<string, DateTime> UserIdDate { get; set; }
    }
}
