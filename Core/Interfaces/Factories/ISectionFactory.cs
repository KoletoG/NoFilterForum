using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFilterForum.Core.Models.DataModels;

namespace Core.Interfaces.Factories
{
    public interface ISectionFactory
    {
        public SectionDataModel Create(string title, string description);
    }
}
