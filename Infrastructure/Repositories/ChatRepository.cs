using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Repositories;
using Core.Models.DataModels;
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
    }
}
