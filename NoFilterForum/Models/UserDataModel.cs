using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NoFilterForum.Models
{
    public class UserDataModel : IdentityUser
    {
        [Key]
        [Required]
        public string Id { get; private init; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public int Posts { get; set; }
        public string Role { get; private init; }
        public short Warnings { get; set; }
    }
}
