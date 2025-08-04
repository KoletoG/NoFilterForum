using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Core.Enums;
using Core.Interfaces.Business_Logic;
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
        public int PostsCount { get; private set; }
        public bool IsConfirmed { get; private set; }
        public UserRoles Role { get; private set; }
        public ISet<string> LikesPostRepliesIds { get; private init; }
        public ISet<string> DislikesPostRepliesIds { get; private init; }
        public ICollection<WarningDataModel> Warnings { get; private init; }
        public DateTime DateCreated { get; private set; }
        public string? Reason { get; private set; }
        public string Bio { get; private set; }
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; private set; }
        public void Confirm() => IsConfirmed = true;
        public void ChangeImageUrl(string imageUrl) => ImageUrl = imageUrl;
        public void IncrementPostCount()
        {
            PostsCount++;
        }
        public void DecrementPostCount()
        {
            PostsCount--;
        }
        public void ChangeBio(string bio) => Bio = bio;
        public void ChangeEmail(string email) => Email = email;
        public void ChangeUsername(string username) => UserName = username;
        public void ChangeRole(UserRoles role) => Role = role;
        public void SetReason(string reason) => Reason = reason;
        public UserDataModel(string userName, string email)
        {
            Id = Nanoid.Generate();
            UserName = userName;
            Email = email;
            PostsCount = 0;
            Role = UserRoles.Newbie;
            IsConfirmed = false;
            Warnings = new List<WarningDataModel>();
            DateCreated = DateTime.UtcNow;
            LikesPostRepliesIds = new HashSet<string>();
            DislikesPostRepliesIds = new HashSet<string>();
            Bio = "";
            ImageUrl = @"images\defaultimage.gif";
        }
        public UserDataModel(string id)
        {
            Id = id;
            Role = UserRoles.Deleted;
            Warnings = new List<WarningDataModel>();
        }
        public UserDataModel()
        {

        }
    }
}
