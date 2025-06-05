using Core.Models.DTOs.InputDTOs;
using Web.ViewModels.Reply;
using Web.ViewModels.Report;

namespace Web.Mappers
{
    public class ReportMapper
    {
        public static CreateReportRequest MapToRequest(CreateReportViewModel vm, string userFromId) => new()
        {
            Content = vm.Content,
            IdOfPostOrReply = vm.IdOfPostReply,
            UserFromId = userFromId,
            UserToId = vm.UserIdTo,
            IsPost = vm.IsPost
        };
    }
}
