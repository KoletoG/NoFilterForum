﻿using System.Web;
using Core.Enums;

namespace Web.ViewModels
{
    public class RoleViewModel
    {
        public UserRoles Role {  get; set; }
        public string UserId { get; set; }
        public bool ShouldRoute {  get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public RoleViewModel(UserRoles role, string userId,string username, bool shouldRoute, string imageUrl)
        {
            Role = role;
            UserId = userId;
            Username = username;
            ShouldRoute = shouldRoute;
            ImageUrl = imageUrl;
        }
    }
}
