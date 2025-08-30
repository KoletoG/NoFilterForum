using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Factories;
using Core.Utility;
using Ganss.Xss;
using NoFilterForum.Core.Models.DataModels;

namespace Infrastructure.Factories
{
    public class ReplyFactory : IReplyFactory
    {
        private readonly IHtmlSanitizer _htmlSanitizer;
        public ReplyFactory(IHtmlSanitizer htmlSanitizer)
        {
            _htmlSanitizer = htmlSanitizer;
            _htmlSanitizer.AllowedTags.Clear();
            _htmlSanitizer.AllowedTags.Add("a");
        }
        public ReplyDataModel Create(string body, UserDataModel user, PostDataModel post)
        {
            body = TextFormatter.LinkCheckText(body);
            var sanitizedFormattedBody = _htmlSanitizer.Sanitize(body);
            return new(sanitizedFormattedBody, user, post);
        }
    }
}
