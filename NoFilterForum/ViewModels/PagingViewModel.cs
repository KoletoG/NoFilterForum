﻿using System.Web;
using Microsoft.Extensions.Primitives;

namespace Web.ViewModels
{
    public class PagingViewModel
    {
        public int Page { get; private init; }
        public int TotalPages { get; private init; }
        public string ControllerName { get; private init; }
        public string ActionName { get; private init; }
        public string Username { get; private init; } = string.Empty;
        public string TitleOfSection { get; private init; } = string.Empty;
        public string Id { get; private init; } = string.Empty;
        public string UserId { get;private init; } = string.Empty;
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;

        public int PreviousPage => Page - 1;
        public int NextPage => Page + 1;

        public PagingViewModel(int page, int totalPages, string controllerName, string actionName, string titleOfSection = "", string id="", string username="", string userId="")
        {
            Page = page;
            TotalPages = totalPages;
            ControllerName = controllerName;
            ActionName = actionName;
            TitleOfSection =titleOfSection;
            Id = id;
            UserId= userId;
            Username = username;
        }
    }

}
