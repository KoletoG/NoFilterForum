using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Core.Utility
{
    public static class TextFormatter
    {
        public static string LinkCheckText(string text)
        {
            return Regex.Replace(text, @"(https?://[^\s]+)", "<a href=\"$1\" target=\"_blank\" rel=\"noopener noreferrer nofollow\">$1</a>");
        }
        public static string ReplaceLinkText(string text)
        {
            return Regex.Replace(text, "<a[^>]*>(.*?)</a>", "$1", RegexOptions.IgnoreCase);
        }
        public static string CheckForHashTags(string text)
        {
            text = Regex.Replace(text, "#.#", "&bull;");
            text = Regex.Replace(text, "##(.+?)##", "<h3>$1</h3>");
            text = Regex.Replace(text, "#(.+?)#", "<b>$1</b>");
            return text;
        }
        public static string MarkTags(string text, string username)
        {
            return Regex.Replace(text, $@"(@{username})", "<span style=\"background-color: #e0e0e0;\">$1</span>");
        }
        public static string[] CheckForTags(string text)
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
        public static string FormatPostBody(string text)
        {
            text = LinkCheckText(text);
            text = CheckForHashTags(text);
            return text;
        }
    }
}
