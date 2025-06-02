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
            string sanitizedFormattedBody = TextFormatter.FormatBody(_htmlSanitizer.Sanitize(body));
            string sanitizedTitle = _htmlSanitizer.Sanitize(title);
            return new(sanitizedTitle, sanitizedFormattedBody,user);
        }
    }
}
