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
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(MessageDataModel messageDataModel, CancellationToken cancellationToken)
        {
            await _context.MessageDataModels.AddAsync(messageDataModel, cancellationToken);
        }
        public async Task<MessageDataModel?> GetByIdAsync(string id)
        {
            return await _context.MessageDataModels.FindAsync(id);
        }
        public void Delete(MessageDataModel messageDataModel)
        {
            _context.MessageDataModels.Remove(messageDataModel);
        }
    }
}
