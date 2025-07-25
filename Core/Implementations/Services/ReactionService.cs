using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Business_Logic;
using Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace Core.Implementations.Services
{
    public class ReactionService : IReactionService
    {
        public void ApplyDislikeLogic<T>(UserDataModel user, T obj, string requestId) where T : ILikeDislike
        {
            var wasLiked = user.LikesPostRepliesIds.Contains(requestId);
            var wasDisliked = user.DislikesPostRepliesIds.Contains(requestId);
            if (wasLiked)
            {
                obj.DecrementLikes();
                user.LikesPostRepliesIds.Remove(requestId);
            }
            if (wasDisliked)
            {
                obj.IncrementLikes();
                user.DislikesPostRepliesIds.Remove(requestId);
            }
            else
            {
                obj.DecrementLikes();
                user.DislikesPostRepliesIds.Add(requestId);
            }
        }
        public void ApplyLikeLogic<T>(UserDataModel user, T obj, string requestId) where T : ILikeDislike
        {
            var wasLiked = user.LikesPostRepliesIds.Contains(requestId);
            var wasDisliked = user.DislikesPostRepliesIds.Contains(requestId);
            if (wasDisliked)
            {
                obj.IncrementLikes();
                user.DislikesPostRepliesIds.Remove(requestId);
            }
            if (wasLiked)
            {
                obj.DecrementLikes();
                user.LikesPostRepliesIds.Remove(requestId);
            }
            else
            {
                obj.IncrementLikes();
                user.LikesPostRepliesIds.Add(requestId);
            }
        }
    }
}
