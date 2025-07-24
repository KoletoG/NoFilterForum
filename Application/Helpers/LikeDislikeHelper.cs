using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Business_Logic;
using NoFilterForum.Core.Models.DataModels;

namespace Application.Helpers
{
    public static class LikeDislikeHelper
    {
        public static void ApplyLikeDislikeLogic<T>(UserDataModel user, T obj, string requestId) where T : ILikeDislike
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
    }
}
