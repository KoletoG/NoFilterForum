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
        public void ApplyDislikeLogic(UserDataModel user, ILikeDislike obj, string requestId)
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
        public void ApplyLikeLogic(UserDataModel user, ILikeDislike obj, string requestId)
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
