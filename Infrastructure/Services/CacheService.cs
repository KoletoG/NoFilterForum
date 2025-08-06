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
                _memoryCache.Set(key, obj, minutes > 0 ? TimeSpan.FromMinutes(minutes) : TimeSpan.FromSeconds(seconds));
            }
            return obj;
        }
        public async Task<T?> TryGetValue<T>(string key, Func<CancellationToken,Task<T>> unitOfWorkMethod, CancellationToken token, int seconds = 15, int minutes = 0)
        {
            if (!_memoryCache.TryGetValue(key, out T? obj))
            {
                obj = await unitOfWorkMethod.Invoke(token);
                _memoryCache.Set(key, obj, minutes > 0 ? TimeSpan.FromMinutes(minutes) : TimeSpan.FromSeconds(seconds));
            }
            return obj;
        }
        public async Task<TOutput?> TryGetValue<TInput,TOutput>(string key, Func<TInput ,Task<TOutput>> unitOfWorkMethod,TInput inputParams, int seconds = 15, int minutes = 0)
        {
            if (!_memoryCache.TryGetValue(key, out TOutput? obj))
            {
                obj = await unitOfWorkMethod.Invoke(inputParams);
                _memoryCache.Set(key, obj, minutes > 0 ? TimeSpan.FromMinutes(minutes) : TimeSpan.FromSeconds(seconds));
            }
            return obj;
        }
        public async Task<T?> TryGetValue<T>(string key, Func<string,Task<T>> unitOfWorkMethod,string param , int seconds = 15, int minutes = 0)
        {
            if (!_memoryCache.TryGetValue(key, out T? obj))
            {
                obj = await unitOfWorkMethod.Invoke(param);
                _memoryCache.Set(key, obj, minutes > 0 ? TimeSpan.FromMinutes(minutes) : TimeSpan.FromSeconds(seconds));
            }
            return obj;
        }
        public async Task<T?> TryGetValue<T>(string key, Func<string, CancellationToken, Task<T>> unitOfWorkMethod, string param,CancellationToken token, int seconds = 15, int minutes = 0)
        {
            if (!_memoryCache.TryGetValue(key, out T? obj))
            {
                obj = await unitOfWorkMethod.Invoke(param, token);
                _memoryCache.Set(key, obj, minutes > 0 ? TimeSpan.FromMinutes(minutes) : TimeSpan.FromSeconds(seconds));
            }
            return obj;
        }
    }
}
