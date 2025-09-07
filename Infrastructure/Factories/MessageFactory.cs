using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Factories;
using Core.Models.DataModels;
using Ganss.Xss;

namespace Infrastructure.Factories
{
    public class MessageFactory(IHtmlSanitizer htmlSanitizer) : IMessageFactory
    {
        private readonly IHtmlSanitizer _htmlSanitizer = htmlSanitizer;
        public MessageDataModel Create(string message, string userId,ChatDataModel chatDataModel)
        {
            return new(_htmlSanitizer.Sanitize(message), userId, chatDataModel);
        }
    }
}
