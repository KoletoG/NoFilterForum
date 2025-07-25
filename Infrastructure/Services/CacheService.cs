using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.OutputDTOs.Admin;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public async Task<T?> TryGetValue<T>(string key, Func<Task<T>> unitOfWorkMethod, int seconds = 15,int minutes=0)
        {
            if (!_memoryCache.TryGetValue(key, out T? obj))
            {
                obj = await unitOfWorkMethod.Invoke();
                _memoryCache.Set(key, obj, minutes > 0 ? TimeSpan.FromMinutes(seconds) : TimeSpan.FromSeconds(seconds));
            }
            return obj;
        }
    }
}
