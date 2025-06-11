using Core.Models.DTOs;
using Core.Models.DTOs.InputDTOs.Warning;
using Core.Models.DTOs.OutputDTOs.Warning;
using Web.ViewModels.Warning;

namespace Web.Mappers
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
