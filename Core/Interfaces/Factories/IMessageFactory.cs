using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.DataModels;

namespace Core.Interfaces.Factories
{
    public interface IMessageFactory
    {
        public MessageDataModel Create(string message, string userId);
    }
}
