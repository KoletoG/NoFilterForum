using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Factories;
using Ganss.Xss;
using NoFilterForum.Core.Models.DataModels;

namespace Infrastructure.Factories
{
    public class ReportFactory : IReportFactory
    {
        private readonly IHtmlSanitizer _htmlSanitizer;
        public ReportFactory(IHtmlSanitizer htmlSanitizer)
        {
            _htmlSanitizer = htmlSanitizer;
            _htmlSanitizer.AllowedTags.Clear();
            _htmlSanitizer.AllowedTags.Add("a");
        }
        public ReportDataModel CreateReport(string content, UserDataModel userFrom, UserDataModel userTo, string idOfPostReply, bool isPost) 
        { 
            content= _htmlSanitizer.Sanitize(content);
            return new(userTo, content, idOfPostReply, isPost, userFrom);
        }
    }
}
