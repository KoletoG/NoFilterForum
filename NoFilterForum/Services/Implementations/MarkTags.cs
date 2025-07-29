using System.Text.RegularExpressions;
using Web.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Web.Services.Implementations
{
    public class MarkTagsService : IMarkTagsService
    {
        public string MarkTags(string text, string currentUsername)
        {
            return string.Join(" ", text.Split(' ')
                .Select(x => Regex.Replace(x, $@"(@{currentUsername})", "<span style=\"background-color: #e0e0e0;\">$1</span>")));
        }
    }
}
