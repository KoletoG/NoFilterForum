using Core.Models.DTOs.OutputDTOs;
using Web.ViewModels;

namespace Web.Mappers.Admin
{
    public static class AdminMappers
    {
        public static UserItemsAdminViewModel MapToViewModel(UserItemForAdminPanelDto dto) => new()
        {
            Email = dto.Email,
            Id = dto.Id,
            Role = dto.Role,
            Username = dto.Username,
            WarningsCount = dto.WarningsCount
        };
    }
}
