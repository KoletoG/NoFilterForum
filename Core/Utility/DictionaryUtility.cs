using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.DTOs.OutputDTOs;
using Microsoft.Extensions.Hosting;

namespace Core.Utility
{
    public class DictionaryUtility
    {
        public static Dictionary<string, DateTime> OrderByDatePostsAndReplies(List<ReplyItemDto> datePosts,
            List<ProfilePostDto> dateReplies)
        {
            Dictionary<string, DateTime> dateOrder = new Dictionary<string, DateTime>();
            foreach (var post in dateReplies)
            {
                dateOrder[post.Id] = post.Created;
            }
            foreach (var reply in datePosts)
            {
                reply.Content = TextFormatter.ReplaceLinkText(reply.Content);
                dateOrder[reply.Id] = reply.Created;
            }
            return dateOrder;
        }
    }
}
