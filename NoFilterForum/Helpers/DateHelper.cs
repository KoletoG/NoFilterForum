using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using Core.Utility;
using Web.ViewModels.Post;
using Web.ViewModels.Reply;

namespace Web.Helpers
{
    public static class DateHelper
    {
        public static Dictionary<string, DateTime> OrderDates(IDictionary<string,PostItemViewModel> postItemDtos, IDictionary<string,ReplyItemViewModel> replyItemDtos, int page, int countPerPage)
        {
            Dictionary<string, DateTime> dateOrder = new Dictionary<string, DateTime>();
            foreach (var post in postItemDtos)
            {
                dateOrder[post.Key] = post.Value.Created;
            }
            foreach (var reply in replyItemDtos)
            {
                reply.Value.Content = TextFormatter.ReplaceLinkText(reply.Value.Content);
                dateOrder[reply.Key] = reply.Value.Created;
            }
            return dateOrder.OrderByDescending(x => x.Value).Skip((page - 1) * countPerPage).Take(countPerPage).ToDictionary();
        }
    }
}
