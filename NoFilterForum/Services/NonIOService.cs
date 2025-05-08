using Microsoft.Extensions.Hosting;
using NoFilterForum.Interfaces;
using System.Text.RegularExpressions;

namespace NoFilterForum.Services
{
    public class NonIOService : INonIOService
    {
        private readonly ILogger<NonIOService> _logger;
        public NonIOService(ILogger<NonIOService> logger)
        {
            _logger = logger;
        }
        public string LinkCheckText(string text)
        {
            return Regex.Replace(text, @"(https?://[^\s]+)", "<a href=\"$1\" target=\"_blank\" rel=\"noopener noreferrer nofollow\">$1</a>");
        }
        public string ReplaceLinkText(string text)
        {
            return Regex.Replace(text, "<a[^>]*>(.*?)</a>", "$1", RegexOptions.IgnoreCase);
        }
        public string CheckForHashTags(string text)
        {
            text = Regex.Replace(text, "#.#", "&bull;");
            text = Regex.Replace(text, "##(.+?)##", "<h3>$1</h3>");
            text = Regex.Replace(text, "#(.+?)#", "<b>$1</b>");
            return text;
        }
        public string[] CheckForTags(string text)
        {
            if (Regex.IsMatch(text, @"@\w+"))
            {
                var matches = Regex.Matches(text, @"@[\w]+");
                string[] names = new string[matches.Count];
                for (int i = 0; i < matches.Count; i++)
                {
                    names[i] = matches[i].Value.Substring(1);
                }
                return names;
            }
            else
            {
                return Array.Empty<string>();
            }
        }
    }
}
