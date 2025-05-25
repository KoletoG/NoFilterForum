using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;

namespace NoFilterForum.Services.Implementations
{
    public class ReplyService : IReplyService
    {
        private readonly IReplyRepository _replyRepository;
        public ReplyService(IReplyRepository replyRepository)
        {
            _replyRepository = replyRepository;
        }
    }
}
