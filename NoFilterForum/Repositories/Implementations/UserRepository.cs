using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Data;
using NoFilterForum.Global_variables;
using NoFilterForum.Repositories.Interfaces;

namespace NoFilterForum.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UserDataModel> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<UserDataModel> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }
        public async Task<List<UserDataModel>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<List<UserDataModel>> GetAllNoDefaultAsync()
        {
           return await _context.Users.AsNoTracking().Where(x => x.UserName != GlobalVariables.DefaultUser.UserName).Include(u => u.Warnings).ToListAsync();
        }
        public async Task<UserDataModel> CreateAsync(UserDataModel user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task UpdateAsync(UserDataModel user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsByNotConfirmedAsync()
        {
            return await _context.Users.AnyAsync(x => !x.IsConfirmed);
        }
        public async Task<List<UserDataModel>> GetAllUnconfirmedAsync()
        {
            return await _context.Users.AsNoTracking().Where(x => !x.IsConfirmed).ToListAsync();
        }
        public async Task DeleteAsync(UserDataModel user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(x=>x.UserName == username);
        }
    }
}
