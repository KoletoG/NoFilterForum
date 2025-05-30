using Core.Enums;
using Core.Models.DTOs.InputDTOs;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IReplyService
    {
        public Task<PostResult> DeleteReplyByIdAsync(DeleteReplyRequest request);
    }
}
