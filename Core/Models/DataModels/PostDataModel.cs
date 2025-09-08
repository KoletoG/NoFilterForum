using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Constants;
using Core.Interfaces.Business_Logic;

namespace NoFilterForum.Core.Models.DataModels
{
    public class PostDataModel : ILikeDislike
    {
        [Key]
        public string Id { get;private set; }
        public string Content { get; private init; }
        public DateTime DateCreated { get; private init; }
        public string Title { get; init; }
        public UserDataModel User { get; private set; }
        public string? UserId { get; private set; }
        public short Likes { get; private set; }
        public string SectionId { get; private set; }
        public ICollection<ReplyDataModel> Replies { get; private init; }
        public bool IsPinned { get; private set; }
        public SectionDataModel Section { get; private set; }
        public void TogglePin() => IsPinned = !IsPinned;
        public void SetDefaultUser() => User = UserConstants.DefaultUser;
        public void IncrementLikes() => Likes++;
        public void DecrementLikes() => Likes--;
        public PostDataModel(string title, string content, UserDataModel user, SectionDataModel sectionDataModel)
        {
            Id = Guid.NewGuid().ToString();
            Content = content;
            Title = title;
            DateCreated = DateTime.UtcNow;
            User = user;
            Replies = new List<ReplyDataModel>();
            Likes = 0;
            IsPinned = false;
            Section = sectionDataModel;
            SectionId = Section.Id;
        }
        private PostDataModel()
        {

        }
    }
}
