using Core.Models.DTOs.InputDTOs;
using Microsoft.Extensions.Configuration.UserSecrets;
using Web.ViewModels.Profile;

namespace Web.Mappers
{
    public class ProfileMapper
    {
        public static ChangeBioRequest MapToRequest(ChangeBioViewModel vm, string userId) => new()
        {
            Bio = vm.Bio,
            UserId = userId
        };
        public static UpdateImageRequest MapToRequest(UpdateImageViewModel vm, string userId) => new()
        {
            Image = vm.Image,
            UserId = userId
        };
        public static GetProfileDtoRequest MapToRequest(string username, string currentUsername) => new()
        {
            Username = username,
            CurrentUsername = currentUsername
        };
    }
}
