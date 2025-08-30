using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Factories;
using Core.Utility;
using Ganss.Xss;
using NoFilterForum.Core.Models.DataModels;

namespace Infrastructure.Factories
{
    public class PostFactory : IPostFactory
    {
        private readonly IHtmlSanitizer _htmlSanitizer;
        public PostFactory(IHtmlSanitizer htmlSanitizer)
        {
            _htmlSanitizer = htmlSanitizer;
        }
        public PostDataModel Create(string title, string body, UserDataModel user)
        {
            _htmlSanitizer.AllowedTags.Add("b");
            _htmlSanitizer.AllowedTags.Add("i");
            _htmlSanitizer.AllowedTags.Add("span");
            _htmlSanitizer.AllowedAttributes.Add("class");
            _htmlSanitizer.AllowedClasses.Add("biggerText");
            body = TextFormatter.LinkCheckText(body);
            string sanitizedFormattedBody = _htmlSanitizer.Sanitize(body);
            string sanitizedTitle = _htmlSanitizer.Sanitize(title);
            return new(sanitizedTitle, sanitizedFormattedBody,user);
        }
    }
}
