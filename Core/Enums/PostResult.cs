using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum PostResult
    {
        Success,
        NotFound,
        UpdateFailed,
        Forbid,
        ValidationProblem,
        Conflict
    }
}
