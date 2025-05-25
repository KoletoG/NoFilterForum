using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NoFilterForum.Data;
using NoFilterForum.Models.DataModels;
using NoFilterForum.Repositories.Interfaces;

namespace NoFilterForum.Repositories.Implementations
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PostDataModel> GetByIdAsync(string id)
        {
            return await _context.PostDataModels.FindAsync(id);
        }
        public async Task<List<PostDataModel>> GetAllByUserAsync(UserDataModel user)
        {
            return await _context.PostDataModels.Where(x => x.User == user).ToListAsync();
        }
        public async Task<List<PostDataModel>> GetAllAsync()
        {
            return await _context.PostDataModels.ToListAsync();
        }
        public async Task<PostDataModel> CreateAsync(PostDataModel post)
        {
            await _context.PostDataModels.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }
        public async Task<int> GetCountByPostIdAsync(string id)
        {
            return await _context.PostDataModels.Where(x => x.Id == id).CountAsync();
        }
        public async Task<bool> UpdateAsync(PostDataModel post)
        { 
            // need error handling
            _context.PostDataModels.Update(post);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task DeleteAsync(PostDataModel post)
        {
            _context.PostDataModels.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}
