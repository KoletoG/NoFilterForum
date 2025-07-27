using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using NoFilterForum.Core.Models.DataModels;

namespace Core.Constants
{
    public static class UserConstants
    {
        public static readonly HashSet<string> adminNames = new() { "Admin" };
        public static readonly UserDataModel DefaultUser = new("DefaultUserId");
    }
}
