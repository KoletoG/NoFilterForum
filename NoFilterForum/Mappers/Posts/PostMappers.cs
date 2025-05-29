using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using Web.ViewModels;

namespace Web.Mappers.Posts
{
    public static class PostMappers
    {
        public static CreatePostRequest MapToRequest(CreatePostViewModel vm, string userId) => new()
        {
            Body = vm.Body,
            Title = vm.Title,
            TitleOfSection = vm.TitleOfSection,
            UserId = userId
        };
        public static PostIndexViewModel MapToViewModel(List<PostIndexItemViewModel> itemVMs, int page, int totalPages, string titleOfSection) => new()
        {
            Posts = itemVMs,
            Page = page,
            TotalPages = totalPages,
            TitleOfSection = titleOfSection
        };
        public static PostIndexItemViewModel MapToViewModel(PostItemDto dto) => new()
        {
            DateCreated = dto.DateCreated,
            Id= dto.Id,
            IsPinned= dto.IsPinned,
            Title = dto.Title,
            Role= dto.Role,
            Username= dto.Username
        };
    }
}
