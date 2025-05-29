using Core.Models.DTOs.InputDTOs;
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
    }
}
