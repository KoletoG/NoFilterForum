using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace NoFilterForum.Models
{
    public class UserDataModel : IdentityUser
    {
        [Key]
        [Required]
        public string Id { get;}
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public int PostsCount { get; set; }
        public List<PostDataModel> Posts { get; set; }
        public List<ReplyDataModel> Replies { get; set; }
        public UserRoles Role { get; private init; }
        public byte Warnings { get; set; }
        public DateTime DateCreated { get; set; }
        
        public UserDataModel(string userName,string email)
        {
            Id = Guid.NewGuid().ToString();
            UserName = userName;
            Email = email;
            PostsCount = 0;
            Posts = new List<PostDataModel>();
            Replies = new List<ReplyDataModel>();
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
