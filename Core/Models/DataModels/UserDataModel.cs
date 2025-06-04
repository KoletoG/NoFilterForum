using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Core.Enums;
using Microsoft.AspNetCore.Identity;
using NanoidDotNet;
using static System.Net.Mime.MediaTypeNames;

namespace NoFilterForum.Core.Models.DataModels
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
        public bool IsConfirmed { get; set; }
        public UserRoles Role { get; set; }
        public List<string> LikesPostRepliesIds { get; set; }
        public List<string> DislikesPostRepliesIds { get; set; }
        public List<WarningDataModel> Warnings { get; set; }
        public DateTime DateCreated { get; set; }
        public string Reason { get; set; }
        public string Bio {  get; set; }
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
        public void Confirm() => IsConfirmed = true;
        public void IncrementPostCount() => PostsCount++;
        public void DecrementPostCount() => PostsCount--;
        public void ChangeBio(string bio) => Bio = bio;
        public void ChangeEmail(string email) => Email = email;
        public void ChangeUsername(string username) => UserName = username;
        public UserDataModel(string userName,string email)
        {
            Id = Nanoid.Generate();
            UserName = userName;
            Email = email;
            PostsCount = 0;
            Role = UserRoles.Newbie;
            IsConfirmed = false;
            Warnings = new List<WarningDataModel>();
            DateCreated = DateTime.UtcNow;
            LikesPostRepliesIds = new List<string>();
            DislikesPostRepliesIds = new List<string>();
            Bio = "";
            ImageUrl = @"\images\defaultimage.gif";
        }
        public UserDataModel()
        {

        }
    }
}
