using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure
{
    public static class RedisProvider
    {
        private readonly  static string connString = "gabvr2019rds001.redis.cache.windows.net:6380,password=VxovT1snpVuu1ewzmzuh7mGEOijxiRCUOo9FjGoxmXs=,ssl=True,abortConnect=False";
        private static ConnectionMultiplexer connection = null;
        public static async Task<Bundle> getConnectionAsync()
        {
            if (connection == null)
                connection = await ConnectionMultiplexer.ConnectAsync(connString);
            
            return new Bundle() { conn = connection };
        }

        public static async Task<object> GetValue(ConnectionMultiplexer connection, int database, string key)
        {
            var cache = connection.GetDatabase(database);
            string value = await cache.StringGetAsync(key);
            return value;
        }

        public static async Task<bool> SetValue(ConnectionMultiplexer connection, int database, string key, string value)
        {
            var cache = connection.GetDatabase(database);
            return await cache.StringSetAsync(key, value);
        }

        public static async Task<object> GetValue(IDatabase database, string key)
        {
            RedisValue value = await database.StringGetAsync(key);
            return value.Box();
        }

        public static async Task<bool> SetValue(IDatabase database, string key, object value)
        {
            return await database.StringSetAsync(key, RedisValue.Unbox(value));
        }

        

    }

    public class Bundle
    {
        public ConnectionMultiplexer conn { get; set; }
    }
}
