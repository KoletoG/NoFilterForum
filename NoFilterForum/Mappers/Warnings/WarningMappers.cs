using Core.Models.DTOs;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using Web.ViewModels;

namespace Web.Mappers.Warnings
{
    public static class WarningMappers
    {
        public static WarningItemViewModel MapToViewModel(WarningsContentDto dto) => new()
        {
            Content = dto.Content
        };
        public static CreateWarningRequest MapToRequest(CreateWarningViewModel vm) => new()
        {
            Content = vm.Content,
            UserId = vm.UserId
        };
    }
}
