﻿using System;
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
        public static HashSet<string> adminNames = new HashSet<string>() { "Admin1" };
        public static UserDataModel DefaultUser = new UserDataModel()
        {
            Id = "DefaultUserId",
            Email = "null@email.c",
            UserName = "default",
            Role = UserRoles.Deleted
        };
    }
}
