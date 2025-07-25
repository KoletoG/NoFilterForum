﻿using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Reply;
using Core.Models.DTOs.OutputDTOs.Reply;
using Web.ViewModels.Reply;

namespace Web.Mappers
{
    public static class ReplyMapper
    {
        public static DeleteReplyRequest MapToRequest(DeleteReplyViewModel vm, string userId) => new(vm.ReplyId, userId);
        public static CreateReplyRequest MapToRequest(CreateReplyViewModel vm, string userId) => new(userId, vm.Content, vm.PostId);
        public static GetListReplyIndexItemRequest MapToRequest(int page, string postId) => new(page, postId);
        public static GetReplyItemRequest MapToRequest(string userId) => new(userId);
        public static ReplyIndexItemViewModel MapToViewModel(ReplyIndexItemDto dto) => new()
        {
            Content = dto.Content,
            Id = dto.Id,
            DateCreated = dto.DateCreated,
            ImageUrl = dto.ImageUrl,
            Likes = dto.Likes,
            Role = dto.Role,
            UserId = dto.UserId,
            Username = dto.Username
        };
        public static PostReplyIndexViewModel MapToViewModel(PostReplyIndexDto dto) => new()
        {
            Id = dto.Id,
            Content = dto.Content,
            DateCreated = dto.DateCreated,
            ImageUrl = dto.ImageUrl,
            Likes = dto.Likes,
            Role = dto.Role,
            UserId = dto.UserId,
            Username = dto.Username,
            Title = dto.Title
        };
        public static CurrentUserReplyIndexViewModel MapToViewModel(CurrentUserReplyIndexDto dto) => new()
        {
            DislikesPostRepliesIds = dto.DislikesPostRepliesIds,
            LikesPostRepliesIds = dto.LikesPostRepliesIds
        };
        public static IndexReplyViewModel MapToViewModel(CurrentUserReplyIndexViewModel userVM,
            PostReplyIndexViewModel postVM,
            List<ReplyIndexItemViewModel> repliesVM,
            int page,
            int totalPages,
            string replyId)
        => new()
        {
            Page = page,
            TotalPages = totalPages,
            ReplyId = replyId,
            CurrentUser = userVM,
            Post = postVM,
            Replies = repliesVM
        };
    }
}
