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
        public ReplyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PostResult> DeleteReplyByIdAsync(DeleteReplyRequest request)
        {
            
            return PostResult.Success;
        }
    }
}
