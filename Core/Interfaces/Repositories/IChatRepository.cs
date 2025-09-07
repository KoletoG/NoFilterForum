using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.DataModels;

namespace Core.Interfaces.Repositories
{
    public interface IChatRepository
    {
        public void Delete(ChatDataModel chatDataModel);
        public IQueryable<ChatDataModel> GetAll();
        public void Update(ChatDataModel chatDataModel);
        public Task CreateAsync(ChatDataModel chatDataModel, CancellationToken cancellationToken);
    }
}
