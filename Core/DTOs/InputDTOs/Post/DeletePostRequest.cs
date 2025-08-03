using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs.Post
{
    public record DeletePostRequest(string PostId, string UserId);
}
