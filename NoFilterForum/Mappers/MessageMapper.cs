using Core.DTOs.InputDTOs.Message;
using Web.ViewModels.Message;

namespace Web.Mappers
{
    public static class MessageMapper
    {
        public static CreateMessageRequest MapToRequest(CreateMessageViewModel vm, string userId) => new(userId, vm.ChatId, vm.Message);
        public static DeleteMessageRequest MapToRequest(DeleteMessageViewModel vm, string userId) => new(vm.ChatId, vm.MessageId, userId);
    }
}
