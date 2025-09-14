using Core.Models.DTOs;
using Core.Models.DTOs.OutputDTOs.Admin;
using Web.ViewModels.Admin;

namespace Web.Mappers
{
    public static class AdminMapper
    {
        public static UserItemsAdminViewModel MapToViewModel(UserForAdminPanelDto dto) => new()
        {
            Email = dto.Email,
            Id = dto.Id,
            Role = dto.Role,
            Username = dto.Username,
            WarningsCount = dto.WarningsCount,
            PostsCount = dto.PostsCount
        };
        public static AdminPanelViewModel MapToViewModel(IEnumerable<UserItemsAdminViewModel> users, bool hasReports, bool notConfirmedExist) => new()
        {
            Users = users,
            HasReports = hasReports,
            NotConfirmedExist = notConfirmedExist
        };
        public static UserReasonViewModel MapToViewModel(UsersReasonsDto dto) => new()
        {
            Username = dto.Username,
            Email = dto.Email,
            Id = dto.Id,
            Reason = dto.Reason
        };
        public static ReasonsViewModel MapToViewModel(IEnumerable<UserReasonViewModel> usersVM) => new()
        {
            Users = usersVM
        };
    }
}
