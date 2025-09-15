using Core.Models.DTOs.InputDTOs.Report;
using Core.Models.DTOs.OutputDTOs.Report;
using Web.ViewModels.Admin;
using Web.ViewModels.Reply;
using Web.ViewModels.Report;

namespace Web.Mappers
{
    public class ReportMapper
    {
        public static CreateReportRequest MapToRequest(CreateReportViewModel vm, string userFromId) => new(vm.UserIdTo, vm.Content, userFromId, vm.IsPost, vm.IdOfPostReply);
        public static DeleteReportRequest MapToRequest(DeleteReportViewModel vm) => new(vm.Id);
        public static ReportItemViewModel MapToViewModel(ReportItemDto dto) => new()
        {
            Content = dto.Content,
            Id = dto.Id,
            IdOfPostReply = dto.IdOfPostReply,
            UserFromUsername = dto.UserFromUsername,
            UserToUsername = dto.UserToUsername
        };
    }
}
