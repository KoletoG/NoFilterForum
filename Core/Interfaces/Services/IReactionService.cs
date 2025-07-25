using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Business_Logic;
using NoFilterForum.Core.Models.DataModels;

namespace Core.Interfaces.Services
{
    public interface IReactionService
    {
        void ApplyDislikeLogic<T>(UserDataModel user, T obj, string requestId) where T : ILikeDislike;
        void ApplyLikeLogic<T>(UserDataModel user, T obj, string requestId) where T : ILikeDislike;
    }
}
