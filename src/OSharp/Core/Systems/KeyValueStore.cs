// -----------------------------------------------------------------------
//  <copyright file="SystemManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-25 21:00</last-date>
// -----------------------------------------------------------------------


namespace OSharp.Core.Systems;

/// <summary>
/// 键值数据存储
/// </summary>
public class KeyValueStore : IKeyValueStore
{
    private readonly IServiceProvider _provider;
    private static readonly Random Random = new Random();
        
    /// <summary>
    /// 初始化一个<see cref="KeyValueStore"/>类型的新实例
    /// </summary>
    public KeyValueStore(IServiceProvider provider)
    {
        _provider = provider;
    }

    private IRepository<KeyValue, long> KeyValueRepository => _provider.GetRequiredService<IRepository<KeyValue, long>>();

    private IDistributedCache Cache => _provider.GetRequiredService<IDistributedCache>();

    /// <summary>
    /// 获取 键值对数据查询数据集
    /// </summary>
    public IQueryable<KeyValue> KeyValues => KeyValueRepository.QueryAsNoTracking();

    /// <summary>
    /// 获取或创建设置信息
    /// </summary>
    /// <typeparam name="TSetting">设置类型</typeparam>
    /// <returns>设置实例，数据库中不存在相应节点时返回默认值</returns>
    public TSetting GetSetting<TSetting>() where TSetting : ISetting, new()
    {
        TSetting setting = new TSetting();
        Type type = typeof(TSetting);
        foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(m => m.PropertyType == typeof(IKeyValue)))
        {
            string key = ((KeyValue)property.GetValue(setting))?.Key;
            if (key == null)
            {
                continue;
            }
            IKeyValue keyValue = GetByKey(key);
            if (keyValue != null)
            {
                property.SetValue(setting, keyValue);
            }
        }
        return setting;
    }

    /// <summary>
    /// 保存设置信息
    /// </summary>
    /// <param name="setting">设置信息</param>
    public async Task<OperationResult> SaveSetting(ISetting setting)
    {
        Check.NotNull(setting, nameof(setting));
            
        Type type = setting.GetType();
        IKeyValue[] keyValues = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType == typeof(IKeyValue))
            .Select(p => (IKeyValue)p.GetValue(setting)).ToArray();
        return await CreateOrUpdate(keyValues);
    }

    /// <summary>
    /// 获取指定根节点的数据项，如输入“System.User.”，则查找所有键以此开头的项
    /// </summary>
    /// <param name="rootKey">根键路径</param>
    /// <returns>多个数据项</returns>
    public IKeyValue[] GetByRootKey(string rootKey)
    {
        string[] keys = GetKeys(rootKey);
        return keys.Select(GetByKey).Where(value => value != null).ToArray();
    }

    /// <summary>
    /// 获取指定键名的数据项，先从缓存中获取，如果不存在，再从数据库查找并缓存
    /// </summary>
    /// <param name="key">键名</param>
    /// <returns>数据项</returns>
    public IKeyValue GetByKey(string key)
    {
        Check.NotNullOrEmpty(key, nameof(key));
            
        // 有效期为 7 ± 1 天，防止缓存集中过期
        int seconds = 7 * 24 * 3600 + Random.Next(-24 * 3600, 24 * 3600);
        string cacheKey = GetCacheKey(key);
        KeyValue value = Cache.Get(cacheKey, () => KeyValueRepository.QueryAsNoTracking(m => m.Key == key, false).FirstOrDefault(), seconds);
        return value;
    }

    /// <summary>
    /// 检查键值对信息是否存在
    /// </summary>
    /// <param name="predicate">检查谓语表达式</param>
    /// <param name="id">更新的键值对信息编号</param>
    /// <returns>键值对信息是否存在</returns>
    public Task<bool> CheckExists(Expression<Func<KeyValue, bool>> predicate, long id = default(long))
    {
        return KeyValueRepository.CheckExistsAsync(predicate, id);
    }

    /// <summary>
    /// 添加或更新键值对信息
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <returns>业务操作结果</returns>
    public Task<OperationResult> CreateOrUpdate(string key, object value)
    {
        Check.NotNullOrEmpty(key, nameof(key));

        IKeyValue pair = new KeyValue(key, value);
        return CreateOrUpdate(pair);
    }

    /// <summary>
    /// 添加或更新键值对信息
    /// </summary>
    /// <param name="dtos">要添加的键值对信息DTO信息</param>
    /// <returns>业务操作结果</returns>
    public async Task<OperationResult> CreateOrUpdate(params IKeyValue[] dtos)
    {
        Check.NotNull(dtos, nameof(dtos));
        if (dtos.Length == 0)
        {
            return OperationResult.NoChanged;
        }

        KeyValue[] values = dtos.OfType<KeyValue>().ToArray();
        List<string> removeKeys = new List<string>();
        IUnitOfWork unitOfWork = _provider.GetUnitOfWork(true);
        foreach (KeyValue dto in values)
        {
            KeyValue pair = KeyValueRepository.Query().FirstOrDefault(m => m.Key == dto.Key);
            if (pair == null)
            {
                pair = dto;
                await KeyValueRepository.InsertAsync(pair);
            }
            else if (pair.Value != dto.Value)
            {
                pair.Value = dto.Value;
                pair.Display = dto.Display;
                pair.Remark = dto.Remark;
                pair.Order = dto.Order;
                pair.IsLocked = dto.IsLocked;
                int count = await KeyValueRepository.UpdateAsync(pair);
                removeKeys.AddIf(pair.Key, count > 0);
            }
        }

        await unitOfWork.CommitAsync();

        string[] cacheKeys = removeKeys.Select(GetCacheKey).ToArray();
        await Cache.RemoveAsync(default, cacheKeys);

        return OperationResult.Success;
    }

    /// <summary>
    /// 删除键值对信息
    /// </summary>
    /// <param name="ids">要删除的键值对信息编号</param>
    /// <returns>业务操作结果</returns>
    public async Task<OperationResult> Delete(params long[] ids)
    {
        Check.NotNull(ids, nameof(ids));

        if (ids.Length == 0)
        {
            return OperationResult.NoChanged;
        }

        List<string> removeKeys = new List<string>();
        IUnitOfWork unitOfWork = _provider.GetUnitOfWork(true);
        foreach (var id in ids)
        {
            KeyValue pair = await KeyValueRepository.GetAsync(id);
            if (pair == null)
            {
                continue;
            }

            int count = await KeyValueRepository.DeleteAsync(pair);
            removeKeys.AddIf(pair.Key, count > 0);
        }

        await unitOfWork.CommitAsync();

        string[] cacheKeys = removeKeys.Select(GetCacheKey).ToArray();
        await Cache.RemoveAsync(default, cacheKeys);

        return OperationResult.Success;
    }

    /// <summary>
    /// 删除以根键路径为起始的所有字典项，如输入“System.User.”，所有键以此开头的项都会被删除
    /// </summary>
    /// <param name="rootKey">根键路径</param>
    /// <returns>业务操作结果</returns>
    public async Task<OperationResult> DeleteByRootKey(string rootKey)
    {
        long[] ids = KeyValueRepository.QueryAsNoTracking(m => m.Key.StartsWith(rootKey)).Select(m => m.Id).ToArray();
        return await Delete(ids);
    }

    private string[] GetKeys(string rootKey)
    {
        string[] keys = KeyValueRepository.QueryAsNoTracking(m => m.Key.StartsWith(rootKey))
            .Select(m => m.Key).Distinct().OrderBy(m => m).ToArray();
        return keys;
    }

    private static string GetCacheKey(string key)
    {
        return $"Systems:KeyValues:{key}";
    }
}
