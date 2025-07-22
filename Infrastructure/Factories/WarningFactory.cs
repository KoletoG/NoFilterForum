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
    public class WarningFactory : IWarningFactory
    {
        private readonly IHtmlSanitizer _htmlSanitizer;
        public WarningFactory(IHtmlSanitizer htmlSanitizer)
        {
            _htmlSanitizer = htmlSanitizer;
            _htmlSanitizer.AllowedTags.Clear();
        }
        public WarningDataModel Create(string content, UserDataModel user)
        {
            string sanitizedContent = _htmlSanitizer.Sanitize(content);
            return new(content,user);
        }
    }
}
