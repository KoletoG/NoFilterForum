using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs
{
    public record CreatePostRequest(string? Title, string? Body, string? TitleOfSection, string? UserId);
}
