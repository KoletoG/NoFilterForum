using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.DataModels;

namespace Core.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        public Task CreateAsync(MessageDataModel messageDataModel, CancellationToken cancellationToken);
        public void Delete(MessageDataModel messageDataModel);
        public Task<MessageDataModel?> GetByIdAsync(string id);
    }
}
