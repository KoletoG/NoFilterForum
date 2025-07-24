using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Business_Logic
{
    public interface ILikeDislike
    {
        void DecrementLikes();
        void IncrementLikes();
    }
}
