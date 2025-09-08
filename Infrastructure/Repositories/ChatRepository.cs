using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Repositories;
using Core.Models.DataModels;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ApplicationDbContext _context;
        public ChatRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<ChatDataModel> GetAll()
        {
            return _context.ChatDataModels;
        }
        public async Task<ChatDataModel?> GetById(string id)
        {
            return await _context.ChatDataModels.FindAsync(id);
        }
        public async Task CreateAsync(ChatDataModel chatDataModel,CancellationToken cancellationToken)
        {
            await _context.ChatDataModels.AddAsync(chatDataModel,cancellationToken);
        }
        public void Delete(ChatDataModel chatDataModel)
        {
            _context.ChatDataModels.Remove(chatDataModel);
        }
        public void Update(ChatDataModel chatDataModel)
        {
            _context.ChatDataModels.Update(chatDataModel);
        }
    }
}
