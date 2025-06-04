using Core.Models.DTOs.InputDTOs;
using Web.ViewModels.Profile;

namespace Web.Mappers
{
    public class ProfileMapper
    {
        public static ChangeBioRequest MapToRequest(ChangeBioViewModel vm) => new()
        {
            Bio = vm.Bio,
            UserId = vm.UserId
        };
    }
}
