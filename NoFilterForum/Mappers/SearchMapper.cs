using Core.DTOs.OutputDTOs.Search;
using Web.ViewModels.Search;

namespace Web.Mappers
{
    public static class SearchMapper
    {
        public static SearchViewModel MapToViewModel(SearchPostDto dto) => new()
        {
            PostBody = dto.Body,
            PostId = dto.Id,
            PostTitle = dto.Title
        };
    }
}
