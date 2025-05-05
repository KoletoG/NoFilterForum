using Microsoft.EntityFrameworkCore;
using NoFilterForum.Data;
using NoFilterForum.Interfaces;
using NoFilterForum.Models;

namespace NoFilterForum.Services
{
    public class IOService : IIOService
    {
        private readonly ApplicationDbContext _context;
        public IOService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UserDataModel?> GetUserByNameAsync(string username)
        {
            return await _context.Users.Include(x=>x.Warnings).FirstOrDefaultAsync(x=>x.UserName==username);
        }
        public async Task AdjustRoleByPostCount(UserDataModel user)
        {

            if(user.Role!=UserRoles.VIP && user.Role != UserRoles.Admin)
            {
                if (user.PostsCount > 500)
                {
                    if (user.Role != UserRoles.Dinosaur)
                    {
                        user.Role = UserRoles.Dinosaur;
                        _context.Attach(user);
                        _context.Entry(user).Property(x => x.Role).IsModified = true;
                        await _context.SaveChangesAsync();
                    }
                }
                else if (user.PostsCount > 20)
                {
                    if (user.Role != UserRoles.Regular)
                    {
                        user.Role = UserRoles.Regular;
                        _context.Attach(user);
                        _context.Entry(user).Property(x => x.Role).IsModified = true;
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    if (user.Role != UserRoles.Newbie)
                    {
                        user.Role = UserRoles.Newbie;
                        _context.Attach(user);
                        _context.Entry(user).Property(x => x.Role).IsModified = true;
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
        public async Task<List<T>> GetTByUserAsync<T>(UserDataModel user) where T: class
        {
            if (typeof(T) == typeof(ReplyDataModel))
            {
                return await _context.ReplyDataModels.Include(x=>x.Post).Where(x => x.User.UserName == user.UserName).OrderByDescending(x => x.DateCreated).ToListAsync() as List<T> ?? new List<T>();
            }
            else if (typeof(T) == typeof(PostDataModel))
            {
                return await _context.PostDataModels.Where(x => x.User.UserName == user.UserName).OrderByDescending(x => x.DateCreated).ToListAsync() as List<T> ?? new List<T>();
            }
            else
            {
                throw new Exception("Invalid datamodel");
            }
        }
        public void DeleteReply(ReplyDataModel replyDataModel)
        {
            replyDataModel.User.PostsCount--;
            _context.ReplyDataModels.Remove(replyDataModel);
        }
        public void DeletePost(PostDataModel postDataModel)
        {
            postDataModel.User.PostsCount--;
            _context.PostDataModels.Remove(postDataModel);
        }
    }
}
