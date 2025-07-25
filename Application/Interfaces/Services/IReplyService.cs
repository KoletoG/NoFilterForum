﻿using Core.Enums;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Reply;
using Core.Models.DTOs.OutputDTOs.Reply;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IReplyService
    {
        public Task<PostResult> DeleteReplyAsync(DeleteReplyRequest request);
        public Task<bool> HasTimeoutByUserIdAsync(string userId);
        public Task<PostResult> LikeAsync(LikeDislikeRequest likeDislikeRequest);
        public Task<PostResult> DislikeAsync(LikeDislikeRequest likeDislikeRequest);
        public Task<List<ReplyIndexItemDto>> GetListReplyIndexItemDto(GetListReplyIndexItemRequest getListReplyIndexItemRequest);
        public Task<(int page, int totalPages)> GetPageAndTotalPage(int page, string postId, string replyId = "");
        public Task<List<ReplyItemDto>> GetListReplyItemDtoAsync(GetReplyItemRequest getReplyItemRequest);
        public Task<PostResult> CreateReplyAsync(CreateReplyRequest createReplyRequest);
    }
}
