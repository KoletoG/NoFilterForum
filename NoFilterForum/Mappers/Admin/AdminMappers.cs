using Core.Models.DTOs;
using Core.Models.DTOs.OutputDTOs;
using Core.Models.ViewModels;
using Web.ViewModels;

namespace Web.Mappers.Admin
{
    public static class AdminMappers
    {
        public static UserItemsAdminViewModel MapToViewModel(UsersForAdminPanelDto dto) => new()
        {
            Email = dto.Email,
            Id = dto.Id,
            Role = dto.Role,
            Username = dto.Username,
            WarningsCount = dto.WarningsCount
        };
        public static ShowWarningsViewModel MapToViewModel(ShowWarningsDto dto) => new()
        {
            Content = dto.Content
        };
    }
}
