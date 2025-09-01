using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs.Post
{
    public record GetIndexPostRequest(string? TitleOfSection,int Page, int PostsCount, string UserId);
}
