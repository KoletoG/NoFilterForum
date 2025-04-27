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
        public UserRoles Role { get; set; }
        public byte Warnings { get; set; }
        public DateTime DateCreated { get; set; }
        
        public UserDataModel(string userName,string email)
        {
            Id=Guid.NewGuid().ToString();
            UserName = userName;
            Email = email;
            PostsCount = 0;
            Role = UserRoles.Newbie;
            Warnings = 0;
            DateCreated = DateTime.Now;
        }
        public UserDataModel()
        {

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
