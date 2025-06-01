using System.Web;
using Core.Models.DTOs.OutputDTOs;
using Web.ViewModels.Section;

namespace Web.Mappers.Section
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
    }
}
