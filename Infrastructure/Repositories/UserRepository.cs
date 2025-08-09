using System.Runtime.CompilerServices;
using Core.Constants;
using Core.Enums;
using Core.Models.DTOs.OutputDTOs.Admin;
using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Models.DataModels;
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
        public async Task<bool> ExistByIdAsync(string id, CancellationToken cancellationToken) => await _context.Users.AnyAsync(u => u.Id == id, cancellationToken);
        public async Task<UserDataModel?> GetByIdAsync(string id)=> await _context.Users.FindAsync(id);
        public async Task<UserDataModel?> GetUserWithWarningsByIdAsync(string id, CancellationToken cancellationToken)=> await _context.Users.Include(x => x.Warnings).FirstOrDefaultAsync(x => x.Id == id,cancellationToken);
        public async Task<UserDataModel?> GetByUsernameAsync(string username)=> await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        public async Task<IReadOnlyCollection<UserDataModel>> GetListByUsernameArrayAsync(string[] usernames, CancellationToken cancellationToken)=> await _context.Users.Where(x=>usernames.Contains(x.UserName)).ToListAsync(cancellationToken);
        public async Task<CurrentUserReplyIndexDto?> GetCurrentUserReplyIndexDtoByIdAsync(string id, CancellationToken cancellationToken)=>await _context.Users.Where(x => x.Id == id).Select(x => new CurrentUserReplyIndexDto(x.LikesPostRepliesIds.ToHashSet(),x.DislikesPostRepliesIds.ToHashSet())).FirstOrDefaultAsync(cancellationToken);
        public async Task<ProfileUserDto?> GetProfileUserDtoByIdAsync(string id, CancellationToken cancellationToken    )=> await _context.Users.Where(x => x.Id == id).Select(x => new ProfileUserDto(x.Id,x.Warnings.Count,x.UserName,x.Email,x.Bio,x.Role,x.PostsCount,x.ImageUrl,x.DateCreated)).FirstOrDefaultAsync(cancellationToken);
        public async Task<IReadOnlyCollection<UserDataModel>> GetAllAsync() => await _context.Users.ToListAsync();
        public async Task<IReadOnlyCollection<UserDataModel>> GetAllNoDefaultAsync()=> await _context.Users.AsNoTracking().Where(x => x.UserName != UserConstants.DefaultUser.UserName).Include(u => u.Warnings).ToListAsync();
        public async Task<IReadOnlyCollection<UserForAdminPanelDto>> GetUserItemsForAdminDtoAsync(CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking()
                .Where(x => x.UserName != UserConstants.DefaultUser.UserName)
                .Select(x => new UserForAdminPanelDto(x.Email,x.Id,x.UserName,x.Warnings.Count,x.Role))
                .ToListAsync(cancellationToken);
        }
        public async Task<bool> ExistNormalizedUsername(string normalizedUsername, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(x=>x.NormalizedUserName == normalizedUsername);
        }
        public async Task<bool> ExistNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(x => x.NormalizedEmail == normalizedEmail, cancellationToken);
        }
        public async Task CreateAsync(UserDataModel user) => await _context.Users.AddAsync(user);
        public void Update(UserDataModel user) => _context.Users.Update(user);
        public void UpdateRange(IEnumerable<UserDataModel> users)=>_context.Users.UpdateRange(users);
        public async Task<UserRoles> GetUserRoleIdAsync(string userId)=> await _context.Users.Where(x=>x.Id==userId).Select(x=>x.Role).FirstOrDefaultAsync();
        public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken) => await _context.Users.AnyAsync(x=>x.UserName == username, cancellationToken);
        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken) => await _context.Users.AnyAsync(x => x.Email == email, cancellationToken);
        public async Task<bool> ExistsByNotConfirmedAsync(CancellationToken cancellationToken) => await _context.Users.AnyAsync(x => !x.IsConfirmed, cancellationToken);
        public async Task<IReadOnlyCollection<UsersReasonsDto>> GetAllUnconfirmedUserDtosAsync(CancellationToken cancellationToken)=> await _context.Users.AsNoTracking().Where(x => !x.IsConfirmed).Select(x=>new UsersReasonsDto(x.Email,x.UserName,x.Reason,x.Id)).ToListAsync(cancellationToken);
        public void Delete(UserDataModel user) => _context.Users.Remove(user);
        public async Task<bool> ExistsByUsernameAsync(string username)=> await _context.Users.AnyAsync(x=>x.UserName == username);
    }
}
