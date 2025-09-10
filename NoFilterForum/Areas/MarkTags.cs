using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Web.Areas 
{
    public class MarkTagsViewModel
    {
        public required string Content { get; set; }
        public void MarkTags(string currentUsername)
        {
            Content = string.Join(" ", Content.Split(' ')
                .Select(x => Regex.Replace(x, $@"(@{currentUsername})", "<span class=\"rounded-1\" style=\"border-top: 1px solid cornflowerblue;border-bottom: 1px solid #B58B64;\">$1</span>")));
        }
    }
}
