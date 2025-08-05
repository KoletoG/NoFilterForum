using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Business_Logic;
using NoFilterForum.Core.Models.DataModels;

namespace Core.Implementations.Services
{
    public static class ReactionService
    {
        public static void ApplyDislikeLogic(UserDataModel user, ILikeDislike obj, string requestId)
        {
            var likesSet = user.LikesPostRepliesIds.ToHashSet();
            var dislikedSet = user.DislikesPostRepliesIds.ToHashSet();
            var wasLiked = likesSet.Contains(requestId);
            var wasDisliked = dislikedSet.Contains(requestId);
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
        public static void ApplyLikeLogic(UserDataModel user, ILikeDislike obj, string requestId)
        {
            var likesSet = user.LikesPostRepliesIds.ToHashSet();
            var dislikedSet = user.DislikesPostRepliesIds.ToHashSet();
            var wasLiked = likesSet.Contains(requestId);
            var wasDisliked = dislikedSet.Contains(requestId);
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
