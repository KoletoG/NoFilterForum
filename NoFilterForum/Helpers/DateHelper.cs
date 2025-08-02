using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using Core.Utility;
using Web.ViewModels.Reply;

namespace Web.Helpers
{
    public static class DateHelper
    {
        public static Dictionary<string, DateTime> OrderDates(IReadOnlyCollection<ProfilePostDto> postItemDtos, List<ReplyItemViewModel> replyItemDtos, int page, int countPerPage)
        {
            Dictionary<string, DateTime> dateOrder = new Dictionary<string, DateTime>();
            foreach (var post in postItemDtos)
            {
                dateOrder[post.Id] = post.Created;
            }
            foreach (var reply in replyItemDtos)
            {
                reply.Content = TextFormatter.ReplaceLinkText(reply.Content);
                dateOrder[reply.Id] = reply.Created;
            }
            return dateOrder.OrderByDescending(x => x.Value).Skip((page - 1) * countPerPage).Take(countPerPage).ToDictionary();
        }
    }
}
