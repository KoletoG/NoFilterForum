using System.Runtime.CompilerServices;
using Core.Enums;
using Core.Models.DTOs.OutputDTOs;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Global_variables;
using NoFilterForum.Infrastructure.Data;

namespace NoFilterForum.Infrastructure.Repositories
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
        public async Task<UserDataModel> GetUserWithWarningsByIdAsync(string id)
        {
            return await _context.Users.Include(x => x.Warnings).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<UserDataModel> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }
        public async Task<ProfileUserDto> GetProfileUserDtoByIdAsync(string id)
        {
            return await _context.Users.Where(x => x.Id == id).Select(x => new ProfileUserDto
            {
                Bio = x.Bio,
                DateCreated = x.DateCreated,
                Email = x.Email,
                Id = x.Id,
                PostsCount = x.PostsCount,
                Role = x.Role,
                UserName = x.UserName,
                WarningsCount = x.Warnings.Count,
                ImageUrl = x.ImageUrl
            }).FirstOrDefaultAsync();
        }
        public async Task<List<UserDataModel>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<List<UserDataModel>> GetAllNoDefaultAsync()
        {
           return await _context.Users.AsNoTracking().Where(x => x.UserName != UserConstants.DefaultUser.UserName).Include(u => u.Warnings).ToListAsync();
        }
        public async Task<List<UserForAdminPanelDto>> GetUserItemsForAdminDtoAsync()
        {
            return await _context.Users.AsNoTracking()
                .Where(x => x.UserName != UserConstants.DefaultUser.UserName)
                .Select(x => new UserForAdminPanelDto(x.Email,x.Id,x.UserName,x.Warnings.Count,x.Role))
                .ToListAsync();
        }
        public async Task<UserDataModel> CreateAsync(UserDataModel user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }
        public async Task UpdateAsync(UserDataModel user)
        {
            _context.Users.Update(user);
        }
        public async Task UpdateRangeAsync(List<UserDataModel> users)
        {
            _context.Users.UpdateRange(users);
        }
        public async Task<UserRoles> GetUserRoleIdAsync(string userId)
        {
            return await _context.Users.Where(x=>x.Id==userId).Select(x=>x.Role).FirstOrDefaultAsync();
        }
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(x=>x.UserName == username);
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
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
        }
        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(x=>x.UserName == username);
        }
    }
}
