using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace Core.Utility
{
    public static class RoleUtility
    {
        public static void AdjustRoleByPostCount(UserDataModel user)
        {

            if (user.Role != UserRoles.VIP && user.Role != UserRoles.Admin)
            {
                if (user.PostsCount > 500)
                {
                    if (user.Role != UserRoles.Dinosaur)
                    {
                        user.ChangeRole(UserRoles.Dinosaur);
                    }
                }
                else if (user.PostsCount > 20)
                {
                    if (user.Role != UserRoles.Regular)
                    {
                        user.ChangeRole(UserRoles.Regular);
                    }
                }
                else
                {
                    if (user.Role != UserRoles.Newbie)
                    {
                        user.ChangeRole(UserRoles.Newbie);
                    }
                }
            }
        }
    }
}
