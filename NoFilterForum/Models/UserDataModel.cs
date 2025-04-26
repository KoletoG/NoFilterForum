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
        public int PostsCount { get; set; }
        public List<PostDataModel> Posts { get; set; }
        public List<ReplyDataModel> Replies { get; set; }
        public UserRoles Role { get; private init; }
        public short Warnings { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public enum UserRoles
    {
        Newbie,
        Regular,
        Dinosaur,
        VIP,
        Admin
    }
}
