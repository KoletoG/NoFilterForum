using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ICacheService
    {
        public Task<T?> TryGetValue<T>(string key, Func<Task<T>> unitOfWorkMethod, int seconds = 15, int minutes = 0);
    }
}
