using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Web.Areas 
{
    public class MarkTagsViewModel
    {
        public string Content { get; set; }
        public void MarkTags(string currentUsername)
        {
            Content = string.Join(" ", Content.Split(' ')
                .Select(x => Regex.Replace(x, $@"(@{currentUsername})", "<span style=\"background-color: #e0e0e0;\">$1</span>")));
        }
    }
}
