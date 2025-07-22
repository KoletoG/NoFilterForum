using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFilterForum.Core.Models.DataModels;

namespace Core.Interfaces.Factories
{
    public interface IWarningFactory
    {
        WarningDataModel Create(string content, UserDataModel user);
    }
}
