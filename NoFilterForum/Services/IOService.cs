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
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x=>x.UserName==username);
        }
    }
}
