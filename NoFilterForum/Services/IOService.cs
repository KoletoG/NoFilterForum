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
            return await _context.Users.FirstOrDefaultAsync(x=>x.UserName==username);
        }
        public async Task AdjustRoleByPostCount(UserDataModel user)
        {
            if (user.PostsCount > 500)
            {
                user.Role = UserRoles.Dinosaur;
                _context.Attach(user);
                _context.Entry(user).Property(x => x.Role).IsModified = true;
                await _context.SaveChangesAsync();
            }
            else if(user.PostsCount>20)
            {
                user.Role = UserRoles.Regular;
                _context.Attach(user);
                _context.Entry(user).Property(x => x.Role).IsModified = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
