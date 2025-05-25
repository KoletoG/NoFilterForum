using NoFilterForum.Repositories.Interfaces;
using NoFilterForum.Services.Interfaces;

namespace NoFilterForum.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
    }
}
