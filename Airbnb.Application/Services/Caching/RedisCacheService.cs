using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Airbnb.Application.Services.Caching
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<T?> GetDataAsync<T>(string key)
        {
            var data = _cache.Get(key);

            if(data is null) 
                return default(T);

            return JsonSerializer.Deserialize<T>(data);
        }

        public async Task SetDataAsync<T>(string key, T data)
        {
            var options = new DistributedCacheEntryOptions 
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            };

            var JsonData = JsonSerializer.Serialize(data);
           await _cache.SetStringAsync(key, JsonData, options);
        }
    }
}
