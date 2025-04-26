using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NoFilterForum.Models
{
    public class UserDataModel : IdentityUser
    {
        [Key]
        [Required]
        public override string Id { get; set; }
        [Required]
        public override string? UserName { get; set; }
        [Required]
        [EmailAddress]
        public override string? Email { get; set; }
        public int PostsCount { get; set; }
        public Stack<PostDataModel> Posts { get; set; }
        public Stack<ReplyDataModel> Replies { get; set; }
        public UserRoles Role { get; private init; }
        public byte Warnings { get; set; }
        public DateTime DateCreated { get; set; }
        
        public UserDataModel(string userName,string email)
        {
            Id = Guid.NewGuid().ToString();
            UserName = userName;
            Email = email;
            PostsCount = 0;
            Posts = new Stack<PostDataModel>();
            Replies = new Stack<ReplyDataModel>();
            Role = UserRoles.Newbie;
            Warnings = 0;
            DateCreated = DateTime.Now;
        }
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
