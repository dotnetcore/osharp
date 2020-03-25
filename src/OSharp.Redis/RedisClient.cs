// -----------------------------------------------------------------------
//  <copyright file="RedisClient.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-26 1:35</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

using OSharp.Core.Options;
using OSharp.Extensions;

using StackExchange.Redis;


namespace OSharp.Redis
{
    /// <summary>
    /// Redis操作客户端
    /// </summary>
    public class RedisClient
    {
        private static readonly IDictionary<string, ConnectionMultiplexer> ConnectionCache = new Dictionary<string, ConnectionMultiplexer>();
        private readonly SemaphoreSlim _connectLock = new SemaphoreSlim(1, 1);

        private volatile ConnectionMultiplexer _connection;
        private readonly int _dbIndex;
        private string _keyPrefix = null;

        /// <summary>
        /// 初始化一个<see cref="RedisClient"/>类型的新实例
        /// </summary>
        public RedisClient(int dbIndex = 0)
        {
            _dbIndex = dbIndex;
            _connection = Connect();
        }

        /// <summary>
        /// 初始化一个<see cref="RedisClient"/>类型的新实例
        /// </summary>
        public RedisClient(string host, int dbIndex = 0)
        {
            _dbIndex = dbIndex;
            _connection = Connect(host);
        }

        #region String

        #region 同步方法

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool StringSet(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddKeyPrefix(key);
            return Do(db => db.StringSet(key, value, expiry));
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public bool StringSet(List<KeyValuePair<string, string>> keyValues)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newKeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(AddKeyPrefix(p.Key), p.Value)).ToList();
            return Do(db => db.StringSet(newKeyValues.ToArray()));
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool StringSet<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddKeyPrefix(key);
            string json = ConvertJson(obj);
            return Do(db => db.StringSet(key, json, expiry));
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public string StringGet(string key)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.StringGet(key));
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        public RedisValue[] StringGet(List<string> listKey)
        {
            List<string> newKeys = listKey.Select(AddKeyPrefix).ToList();
            return Do(db => db.StringGet(ConvertRedisKeys(newKeys)));
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public T StringGet<T>(string key)
        {
            key = AddKeyPrefix(key);
            return Do(db => ConvertObj<T>(db.StringGet(key)));
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double StringIncrement(string key, double val = 1)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.StringIncrement(key, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double StringDecrement(string key, double val = 1)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.StringDecrement(key, val));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddKeyPrefix(key);
            return await Do(db => db.StringSetAsync(key, value, expiry));
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync(List<KeyValuePair<string, string>> keyValues)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newKeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(AddKeyPrefix(p.Key), p.Value)).ToList();
            return await Do(db => db.StringSetAsync(newKeyValues.ToArray()));
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public async Task<bool> StringSetAsync<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddKeyPrefix(key);
            string json = ConvertJson(obj);
            return await Do(db => db.StringSetAsync(key, json, expiry));
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public async Task<string> StringGetAsync(string key)
        {
            key = AddKeyPrefix(key);
            return await Do(db => db.StringGetAsync(key));
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        public async Task<RedisValue[]> StringGetAsync(List<string> listKey)
        {
            List<string> newKeys = listKey.Select(AddKeyPrefix).ToList();
            return await Do(db => db.StringGetAsync(ConvertRedisKeys(newKeys)));
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public async Task<T> StringGetAsync<T>(string key)
        {
            key = AddKeyPrefix(key);
            string result = await Do(db => db.StringGetAsync(key));
            return ConvertObj<T>(result);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> StringIncrementAsync(string key, double val = 1)
        {
            key = AddKeyPrefix(key);
            return await Do(db => db.StringIncrementAsync(key, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> StringDecrementAsync(string key, double val = 1)
        {
            key = AddKeyPrefix(key);
            return await Do(db => db.StringDecrementAsync(key, val));
        }

        #endregion 异步方法

        #endregion String

        #region List

        #region 同步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="value"></param>
        public void ListRemove<T>(string key, T value)
        {
            key = AddKeyPrefix(key);
            Do(db => db.ListRemove(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public List<T> ListRange<T>(string key)
        {
            key = AddKeyPrefix(key);
            return Do(redis =>
            {
                var values = redis.ListRange(key);
                return ConvertList<T>(values);
            });
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="value"></param>
        public void ListRightPush<T>(string key, T value)
        {
            key = AddKeyPrefix(key);
            Do(db => db.ListRightPush(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public T ListRightPop<T>(string key)
        {
            key = AddKeyPrefix(key);
            return Do(db =>
            {
                var value = db.ListRightPop(key);
                return ConvertObj<T>(value);
            });
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <param name="value"></param>
        public void ListLeftPush<T>(string key, T value)
        {
            key = AddKeyPrefix(key);
            Do(db => db.ListLeftPush(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public T ListLeftPop<T>(string key)
        {
            key = AddKeyPrefix(key);
            return Do(db =>
            {
                var value = db.ListLeftPop(key);
                return ConvertObj<T>(value);
            });
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public long ListLength(string key)
        {
            key = AddKeyPrefix(key);
            return Do(redis => redis.ListLength(key));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="value"></param>
        public async Task<long> ListRemoveAsync<T>(string key, T value)
        {
            key = AddKeyPrefix(key);
            return await Do(db => db.ListRemoveAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public async Task<List<T>> ListRangeAsync<T>(string key)
        {
            key = AddKeyPrefix(key);
            var values = await Do(redis => redis.ListRangeAsync(key));
            return ConvertList<T>(values);
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="value"></param>
        public async Task<long> ListRightPushAsync<T>(string key, T value)
        {
            key = AddKeyPrefix(key);
            return await Do(db => db.ListRightPushAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public async Task<T> ListRightPopAsync<T>(string key)
        {
            key = AddKeyPrefix(key);
            var value = await Do(db => db.ListRightPopAsync(key));
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <param name="value"></param>
        public async Task<long> ListLeftPushAsync<T>(string key, T value)
        {
            key = AddKeyPrefix(key);
            return await Do(db => db.ListLeftPushAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public async Task<T> ListLeftPopAsync<T>(string key)
        {
            key = AddKeyPrefix(key);
            var value = await Do(db => db.ListLeftPopAsync(key));
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public async Task<long> ListLengthAsync(string key)
        {
            key = AddKeyPrefix(key);
            return await Do(redis => redis.ListLengthAsync(key));
        }

        #endregion 异步方法

        #endregion List

        #region Hash

        #region 同步方法

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="dataKey">数据键</param>
        /// <returns></returns>
        public bool HashExists(string key, string dataKey)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.HashExists(key, dataKey));
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <param name="dataKey">数据键</param>
        /// <param name="t">要存储的数据</param>
        /// <returns></returns>
        public bool HashSet<T>(string key, string dataKey, T t)
        {
            key = AddKeyPrefix(key);
            return Do(db =>
            {
                string json = ConvertJson(t);
                return db.HashSet(key, dataKey, json);
            });
        }

        /// <summary>
        /// 批量存储数据到hash表
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <param name="items">要存储的数据键值集合</param>
        /// <returns></returns>
        public bool HashSet<T>(string key, List<KeyValuePair<string, T>> items)
        {
            key = AddKeyPrefix(key);
            return Do(db =>
            {
                HashEntry[] entries = items.Select(m => new HashEntry(m.Key, ConvertJson(m.Value))).ToArray();
                db.HashSet(key, entries);
                return true;
            });
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="dataKey">要移除的数据键</param>
        /// <returns></returns>
        public bool HashDelete(string key, string dataKey)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.HashDelete(key, dataKey));
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public long HashDelete(string key, string[] dataKeys)
        {
            key = AddKeyPrefix(key);
            return Do(db =>
            {
                RedisValue[] keys = dataKeys.Select(m => (RedisValue)m).ToArray();
                return db.HashDelete(key, keys);
            });
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <param name="dataKey">数据键</param>
        /// <returns></returns>
        public T HashGet<T>(string key, string dataKey)
        {
            key = AddKeyPrefix(key);
            return Do(db =>
            {
                string value = db.HashGet(key, dataKey);
                return ConvertObj<T>(value);
            });
        }

        /// <summary>
        /// 从hash表获取多个数据
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <param name="dataKeys">数据键集合</param>
        /// <returns></returns>
        public T[] HashGet<T>(string key, string[] dataKeys)
        {
            key = AddKeyPrefix(key);
            return Do(db =>
            {
                RedisValue[] keys = dataKeys.Select(m => (RedisValue)m).ToArray();
                RedisValue[] values = db.HashGet(key, keys);
                return values.Select(ConvertObj<T>).ToArray();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public T[] HashGetAll<T>(string key)
        {
            key = AddKeyPrefix(key);
            return Do(db =>
            {
                HashEntry[] entries = db.HashGetAll(key);
                return entries.Select(m => ConvertObj<T>(m.Value)).ToArray();
            });
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="dataKey">数据键</param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double HashIncrement(string key, string dataKey, double val = 1)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.HashIncrement(key, dataKey, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="dataKey">数据键</param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double HashDecrement(string key, string dataKey, double val = 1)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.HashDecrement(key, dataKey, val));
        }

        /// <summary>
        /// 获取HashKey所有Redis key
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public List<T> HashKeys<T>(string key)
        {
            key = AddKeyPrefix(key);
            return Do(db =>
            {
                RedisValue[] values = db.HashKeys(key);
                return ConvertList<T>(values);
            });
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="dataKey">数据键</param>
        /// <returns></returns>
        public async Task<bool> HashExistsAsync(string key, string dataKey)
        {
            key = AddKeyPrefix(key);
            return await Do(db => db.HashExistsAsync(key, dataKey));
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <param name="dataKey">数据键</param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<bool> HashSetAsync<T>(string key, string dataKey, T t)
        {
            key = AddKeyPrefix(key);
            return await Do(db =>
            {
                string json = ConvertJson(t);
                return db.HashSetAsync(key, dataKey, json);
            });
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="dataKey">数据键</param>
        /// <returns></returns>
        public async Task<bool> HashDeleteAsync(string key, string dataKey)
        {
            key = AddKeyPrefix(key);
            return await Do(db => db.HashDeleteAsync(key, dataKey));
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public async Task<long> HashDeleteAsync(string key, string[] dataKeys)
        {
            key = AddKeyPrefix(key);
            //List<RedisValue> dataKeys1 = new List<RedisValue>() {"1","2"};
            return await Do(db =>
            {
                RedisValue[] keys = dataKeys.Select(m => (RedisValue)m).ToArray();
                return db.HashDeleteAsync(key, keys);
            });
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <param name="dataKey">数据键</param>
        /// <returns></returns>
        public async Task<T> HashGetAsync<T>(string key, string dataKey)
        {
            key = AddKeyPrefix(key);
            string value = await Do(db => db.HashGetAsync(key, dataKey));
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="dataKey">数据键</param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> HashIncrementAsync(string key, string dataKey, double val = 1)
        {
            key = AddKeyPrefix(key);
            return await Do(db => db.HashIncrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="dataKey">数据键</param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> HashDecrementAsync(string key, string dataKey, double val = 1)
        {
            key = AddKeyPrefix(key);
            return await Do(db => db.HashDecrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 获取HashKey所有Redis key
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public async Task<List<T>> HashKeysAsync<T>(string key)
        {
            key = AddKeyPrefix(key);
            RedisValue[] values = await Do(db => db.HashKeysAsync(key));
            return ConvertList<T>(values);
        }

        #endregion 异步方法

        #endregion Hash

        #region SortedSet 有序集合

        #region 同步方法

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public bool SortedSetAdd<T>(string key, T value, double score)
        {
            key = AddKeyPrefix(key);
            return Do(redis => redis.SortedSetAdd(key, ConvertJson(value), score));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="value"></param>
        public bool SortedSetRemove<T>(string key, T value)
        {
            key = AddKeyPrefix(key);
            return Do(redis => redis.SortedSetRemove(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public List<T> SortedSetRangeByRank<T>(string key)
        {
            key = AddKeyPrefix(key);
            return Do(redis =>
            {
                var values = redis.SortedSetRangeByRank(key);
                return ConvertList<T>(values);
            });
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public long SortedSetLength(string key)
        {
            key = AddKeyPrefix(key);
            return Do(redis => redis.SortedSetLength(key));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        public async Task<bool> SortedSetAddAsync<T>(string key, T value, double score)
        {
            key = AddKeyPrefix(key);
            return await Do(redis => redis.SortedSetAddAsync(key, ConvertJson(value), score));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <param name="value"></param>
        public async Task<bool> SortedSetRemoveAsync<T>(string key, T value)
        {
            key = AddKeyPrefix(key);
            return await Do(redis => redis.SortedSetRemoveAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public async Task<List<T>> SortedSetRangeByRankAsync<T>(string key)
        {
            key = AddKeyPrefix(key);
            var values = await Do(redis => redis.SortedSetRangeByRankAsync(key));
            return ConvertList<T>(values);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">数据集键</param>
        /// <returns></returns>
        public async Task<long> SortedSetLengthAsync(string key)
        {
            key = AddKeyPrefix(key);
            return await Do(redis => redis.SortedSetLengthAsync(key));
        }

        #endregion 异步方法

        #endregion SortedSet 有序集合

        #region key

        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public bool KeyDelete(string key)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.KeyDelete(key));
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">Redis Key</param>
        /// <returns>成功删除的个数</returns>
        public long KeyDelete(List<string> keys)
        {
            List<string> newKeys = keys.Select(AddKeyPrefix).ToList();
            return Do(db => db.KeyDelete(ConvertRedisKeys(newKeys)));
        }

        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public bool KeyExists(string key)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.KeyExists(key));
        }

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public bool KeyRename(string key, string newKey)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.KeyRename(key, newKey));
        }

        /// <summary>
        /// 设置Key的时间
        /// </summary>
        /// <param name="key">redis key</param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddKeyPrefix(key);
            return Do(db => db.KeyExpire(key, expiry));
        }

        #endregion key

        #region 发布订阅

        /// <summary>
        /// Redis发布订阅  订阅
        /// </summary>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        public void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            ISubscriber sub = _connection.GetSubscriber();
            sub.Subscribe(subChannel,
                (channel, message) =>
                {
                    if (handler == null)
                    {
                        Console.WriteLine(subChannel + " 订阅收到消息：" + message);
                    }
                    else
                    {
                        handler(channel, message);
                    }
                });
        }

        /// <summary>
        /// Redis发布订阅  发布
        /// </summary>
        /// <typeparam name="T">数据项类型</typeparam>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public long Publish<T>(string channel, T msg)
        {
            ISubscriber sub = _connection.GetSubscriber();
            return sub.Publish(channel, ConvertJson(msg));
        }

        /// <summary>
        /// Redis发布订阅  取消订阅
        /// </summary>
        /// <param name="channel"></param>
        public void Unsubscribe(string channel)
        {
            ISubscriber sub = _connection.GetSubscriber();
            sub.Unsubscribe(channel);
        }

        /// <summary>
        /// Redis发布订阅  取消全部订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            ISubscriber sub = _connection.GetSubscriber();
            sub.UnsubscribeAll();
        }

        #endregion 发布订阅

        #region 其他

        /// <summary>
        /// 创建一个事务，返回一个IRedisTransaction对象
        /// </summary>
        /// <returns></returns>
        public ITransaction CreateTransaction()
        {
            return GetDatabase().CreateTransaction();
        }

        /// <summary>
        /// 获取当前操作的数据库
        /// </summary>
        /// <returns></returns>
        public IDatabase GetDatabase()
        {
            return _connection.GetDatabase(_dbIndex);
        }

        /// <summary>
        /// 获取当前的服务器
        /// </summary>
        /// <param name="hostAndPort"></param>
        /// <returns></returns>
        public IServer GetServer(string hostAndPort)
        {
            return _connection.GetServer(hostAndPort);
        }

        /// <summary>
        /// 设置自定义前缀
        /// </summary>
        /// <param name="prefix">自定义前缀</param>
        public void SetKeyPrefix(string prefix)
        {
            _keyPrefix = prefix;
        }

        /// <summary>
        /// 给指定键的数据加锁
        /// </summary>
        /// <param name="key">在数据库中的唯一键值</param>
        /// <param name="token">加锁的标记，表示锁的拥有者，并负责解锁</param>
        /// <param name="duration">锁的持续时间</param>
        /// <returns></returns>
        public bool LockTake(string key, string token, TimeSpan duration)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.LockTake(key, token, duration));
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key">在数据库中的唯一键值</param>
        /// <param name="token">加锁的标记，表示锁的拥有者，并负责解锁</param>
        public bool LockRelease(string key, string token)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.LockRelease(key, token));
        }

        /// <summary>
        /// 延伸一把锁
        /// </summary>
        /// <param name="key">在数据库中的唯一键值</param>
        /// <param name="token">加锁的标记，表示锁的拥有者，并负责解锁</param>
        /// <param name="expiry">延伸的时间</param>
        public bool LockExtend(string key, string token, TimeSpan expiry)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.LockExtend(key, token, expiry));
        }

        /// <summary>
        /// 查询一把锁的标记，锁的拥有者
        /// </summary>
        /// <param name="key">在数据库中的唯一键值</param>
        public string LockQuery(string key)
        {
            key = AddKeyPrefix(key);
            return Do(db => db.LockQuery(key));
        }

        /// <summary>
        /// 等待锁，并执行指定功能
        /// </summary>
        /// <typeparam name="T">项类型</typeparam>
        /// <param name="key">在数据库中的唯一键值</param>
        /// <param name="duration">单锁持续时间，默认10秒</param>
        /// <param name="getDataFunc">从Redis中获取数据的操作</param>
        /// <param name="setDataAction">将数据保存回Redis的操作</param>
        /// <param name="lockTimeout">锁超时时间，默认120秒</param>
        /// <returns></returns>
        public T LockWait<T>(string key, Func<T> getDataFunc, Action<T> setDataAction = null, TimeSpan? duration = null, TimeSpan? lockTimeout = null) where T : class
        {
            key = AddKeyPrefix(key) + "_lock";
            string token = Guid.NewGuid().ToString();
            lockTimeout = lockTimeout ?? TimeSpan.FromSeconds(120);
            return Do(db =>
            {
                DateTime now = DateTime.Now;
                while (true)
                {
                    if (!db.LockTake(key, token, duration ?? TimeSpan.FromSeconds(10)))
                    {
                        Thread.Sleep(200);
                        if (DateTime.Now.Subtract(now) > lockTimeout)
                        {
                            throw new TimeoutException($"Redis并发锁的超时时间({lockTimeout.Value.Seconds}秒)已到");
                        }
                        continue;
                    }
                    try
                    {
                        T result = getDataFunc();
                        if (result != null && setDataAction != null)
                        {
                            setDataAction(result);
                        }
                        return result;
                    }
                    finally
                    {
                        db.LockRelease(key, token);
                    }
                }
            });
        }

        #endregion 其他


        #region 私有方法

        private RedisOptions GetOptions()
        {
            RedisOptions options = new RedisOptions();
            return AppSettingsReader.GetInstance("OSharp:Redis", options);
        }

        private ConnectionMultiplexer Connect(string host = null)
        {
            if (string.IsNullOrEmpty(host))
            {
                RedisOptions options = GetOptions();
                host = options != null ? options.Configuration : "localhost:6379";
            }

            _connectLock.Wait();
            try
            {
                if (ConnectionCache.TryGetValue(host, out ConnectionMultiplexer connection) && connection.IsConnected)
                {
                    return connection;
                }

                connection = ConnectionMultiplexer.Connect(host);
                ConnectionCache[host] = connection;
                return connection;
            }
            finally
            {
                _connectLock.Release();
            }
        }

        private T Do<T>(Func<IDatabase, T> func)
        {
            var database = _connection.GetDatabase(_dbIndex);
            return func(database);
        }

        private string AddKeyPrefix(string oldKey)
        {
            if (string.IsNullOrEmpty(_keyPrefix))
            {
                return oldKey;
            }
            return $"{_keyPrefix}:{oldKey}";
        }

        private static string ConvertJson<T>(T value)
        {
            if (value == null)
            {
                return null;
            }
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }

        private static T ConvertObj<T>(RedisValue value)
        {
            if (value == RedisValue.Null)
            {
                return default(T);
            }
            if (typeof(T) == typeof(string))
            {
                return value.CastTo<T>();
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        private static List<T> ConvertList<T>(IEnumerable<RedisValue> values)
        {
            return values.Select(ConvertObj<T>).ToList();
        }

        private static RedisKey[] ConvertRedisKeys(IEnumerable<string> redisKeys)
        {
            return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        }


        #endregion
    }
}