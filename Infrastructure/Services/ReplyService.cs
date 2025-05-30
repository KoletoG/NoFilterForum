using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class ReplyService : IReplyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public ReplyService(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }
        public async Task<PostResult> DeleteReplyByIdAsync(DeleteReplyRequest request)
        {
            bool isAdmin = await _userService.IsAdminRoleByIdAsync(request.UserId);
            if (!isAdmin)
            {
                string replyUserId = await _unitOfWork.Replies.GetUserIdByReplyIdAsync(request.ReplyId);
                if(replyUserId != request.UserId)
                {
                    return PostResult.Forbid;
                }

            }
            return PostResult.Success;
        }
    }
}
