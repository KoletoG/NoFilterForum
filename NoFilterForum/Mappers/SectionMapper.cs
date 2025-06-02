using System.Web;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using NoFilterForum.Core.Models.ViewModels;
using Web.ViewModels.Section;

namespace Web.Mappers
{
    public static class SectionMapper
    {
        public static SectionItemViewModel MapToViewModel(SectionItemDto dto) => new()
        {
            Description = dto.Description,
            Id = dto.Id,
            Title = dto.Title,
            EncodedTitle = HttpUtility.UrlEncode(dto.Title)
        };
        public static IndexSectionViewModel MapToViewModel(List<SectionItemViewModel> itemVMs, bool isAdmin) => new()
        {
            Sections = itemVMs,
            IsAdmin = isAdmin
        };
        public static CreateSectionRequest MapToRequest(CreateSectionViewModel vm, string userId) => new()
        {
            Title = vm.Title,
            Description = vm.Description,
            UserId = userId
        };
        public static DeleteSectionRequest MapToRequest(DeleteSectionViewModel vm) => new()
        {
            SectionId = vm.SectionId
        };
    }
}
