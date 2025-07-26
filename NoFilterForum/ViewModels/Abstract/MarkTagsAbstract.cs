using System.Text.RegularExpressions;

namespace Web.ViewModels.Abstract
{
    public abstract class MarkTagsAbstract
    {
        public string Content { get; set; }
        private string MarkTags(string text, string username)
        {
            return Regex.Replace(text, $@"(@{username})", "<span style=\"background-color: #e0e0e0;\">$1</span>");
        }
        public void MarkTagsOfContent(string currentUsername)
        {
            Content = string.Join(" ", Content.Split(' ').Select(x => MarkTags(x, currentUsername)));
        }
    }
}
