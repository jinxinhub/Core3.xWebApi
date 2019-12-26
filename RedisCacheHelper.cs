using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Core3.xWebApi
{
    public class RedisCacheHelper
    {
        private  static ConnectionMultiplexer redisConnection { get; set; }
        private IDatabase DB { get; set; }

        public RedisCacheHelper()
        {
            redisConnection = ConnectionMultiplexer.Connect("127.0.0.1:6379,password=123456");
            DB = redisConnection.GetDatabase(1);
        }

        /// <summary>
        /// 添加缓存(单个字符串)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> SetCacheString(string key,string value)
        {
            return await DB.StringSetAsync(key, value);
        }

        /// <summary>
        /// 底部插入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> SetCacheListRightPush(RedisKey key, string value)
        {
            return await DB.ListRightPushAsync(key, value);
        }

        /// <summary>
        /// 底部获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetCacheListRight(string key)
        {
            //底部获取一个
           return  await DB.ListRightPopAsync(key);
        }
    }
}
