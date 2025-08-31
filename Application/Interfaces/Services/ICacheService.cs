using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ICacheService
    {
        public void Remove(string key);
        public Task<TOutput?> TryGetValue<TInput, TOutput>(string key, Func<TInput, CancellationToken, Task<TOutput>> unitOfWorkMethod, TInput inputParams, CancellationToken token, int seconds = 15, int minutes = 0);
        public Task<T?> TryGetValue<T>(string key, Func<Task<T>> unitOfWorkMethod, int seconds = 15, int minutes = 0);
        public Task<TOutput?> TryGetValue<TInput, TOutput>(string key, Func<TInput, Task<TOutput>> unitOfWorkMethod, TInput inputParams, int seconds = 15, int minutes = 0);
        public Task<T?> TryGetValue<T>(string key, Func<CancellationToken, Task<T>> unitOfWorkMethod, CancellationToken token, int seconds = 15, int minutes = 0);
        public Task<T?> TryGetValue<T>(string key, Func<string, CancellationToken, Task<T>> unitOfWorkMethod, string param, CancellationToken token, int seconds = 15, int minutes = 0);
    }
}
