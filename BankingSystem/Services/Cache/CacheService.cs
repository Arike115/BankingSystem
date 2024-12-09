﻿using StackExchange.Redis;
using System.Text.Json;

namespace BankingSystem.Services.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IDatabase _cache;

        public CacheService(IConnectionMultiplexer redisConnection)
        {
            this._redisConnection = redisConnection;
            _cache = redisConnection.GetDatabase();
        }

        public void ClearAll()
        {
            var endpoints = _redisConnection.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _redisConnection.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }

        public async Task ClearAsync(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var result = await _cache.StringGetAsync(key);
            if (result.IsNull)
                return default;

            return JsonSerializer.Deserialize<T>(result);
        }

        public async Task<bool> SetAsync<T>(string key, T value, int duration)
        {
            var stringValue = JsonSerializer.Serialize(value);
            var expirationTime = TimeSpan.FromMinutes(duration);

            return await _cache.StringSetAsync(key, stringValue, expirationTime);
        }

      
    }
}