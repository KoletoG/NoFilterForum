using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Factories;
using Ganss.Xss;
using NoFilterForum.Core.Models.DataModels;

namespace Infrastructure.Factories
{
    public class SectionFactory : ISectionFactory
    {
        private readonly IHtmlSanitizer _htmlSanitizer;
        public SectionFactory(IHtmlSanitizer htmlSanitizer)
        {
            _htmlSanitizer = htmlSanitizer;
        }
        public SectionDataModel Create(string title, string description)
        {
            title = _htmlSanitizer.Sanitize(title);
            description = _htmlSanitizer.Sanitize(description);
            return new SectionDataModel(title, description);
        }
    }
}
