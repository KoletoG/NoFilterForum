using NoFilterForum.Repositories.Interfaces;
using NoFilterForum.Services.Interfaces;

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
