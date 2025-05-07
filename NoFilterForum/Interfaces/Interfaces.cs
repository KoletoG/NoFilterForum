using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Interfaces
{
    public interface IIOService
    {
        Task<UserDataModel?> GetUserByNameAsync(string username);
        Task AdjustRoleByPostCount(UserDataModel user);
        Task<List<T>> GetTByUserAsync<T>(UserDataModel user) where T : class;
        Task DeleteReply(ReplyDataModel replyDataModel);
        Task DeletePost(PostDataModel postDataModel);
        Task<UserDataModel> GetUserByIdAsync(string id);
    }
    public interface INonIOService
    {
        public string LinkCheckText(string text);
        public string ReplaceLinkText(string text); 
        public string CheckForHashTags(string text);
        public string[] CheckForTags(string text);
    }
}
