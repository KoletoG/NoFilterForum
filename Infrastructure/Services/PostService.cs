using Core.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;

namespace NoFilterForum.Infrastructure.Services
{
   

    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<PostService> _logger;
        public PostService(IPostRepository postRepository, ILogger<PostService> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }
        public async Task<PinPostResult> PinPostAsync(string postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                _logger.LogError($"Post with ID: {postId} was not found.");
                return PinPostResult.NotFound;
            }
            post.TogglePin();
            var updated = await _postRepository.UpdateAsync(post);
            if (!updated)
            {
                _logger.LogError($"Problem updating the post object with ID: {postId}.");
                return PinPostResult.UpdateFailed;
            }
            return PinPostResult.Success;
        }
    }
}
