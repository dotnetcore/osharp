// -----------------------------------------------------------------------
//  <copyright file="DistributedCacheExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-17 16:45</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Caching;

/// <summary>
/// <see cref="IDistributedCache"/>扩展方法
/// </summary>
public static class DistributedCacheExtensions
{
    /// <summary>
    /// 将对象存入缓存中，使用功能配置
    /// </summary>
    public static void Set(this IDistributedCache cache, string key, object value, IFunction function)
    {
        Check.NotNullOrEmpty(key, nameof(key));
        Check.NotNull(value, nameof(value));
        Check.NotNull(function, nameof(function));

        DistributedCacheEntryOptions options = function.ToCacheOptions();
        if (options == null)
        {
            return;
        }
        cache.Set(key, value, options);
    }

    /// <summary>
    /// 异步将对象存入缓存中，使用功能配置
    /// </summary>
    public static Task SetAsync(this IDistributedCache cache, string key, object value, IFunction function)
    {
        Check.NotNullOrEmpty(key, nameof(key));
        Check.NotNull(value, nameof(value));
        Check.NotNull(function, nameof(function));

        DistributedCacheEntryOptions options = function.ToCacheOptions();
        if (options == null)
        {
            return Task.FromResult(0);
        }
        return cache.SetAsync(key, value, options);
    }
        
    /// <summary>
    /// 获取指定键的缓存项，不存在则从指定委托获取，并回存到缓存中再返回
    /// </summary>
    public static TResult Get<TResult>(this IDistributedCache cache, string key, Func<TResult> getFunc, IFunction function)
    {
        DistributedCacheEntryOptions options = function.ToCacheOptions();
        if (options == null)
        {
            return getFunc();
        }
        return cache.Get<TResult>(key, getFunc, options);
    }

    /// <summary>
    /// 获取指定键的缓存项，不存在则从指定委托获取，并回存到缓存中再返回
    /// </summary>
    public static Task<TResult> GetAsync<TResult>(this IDistributedCache cache, string key, Func<Task<TResult>> getAsyncFunc, IFunction function)
    {
        DistributedCacheEntryOptions options = function.ToCacheOptions();
        if (options == null)
        {
            return getAsyncFunc();
        }
        return cache.GetAsync<TResult>(key, getAsyncFunc, options);
    }
        
    /// <summary>
    /// 将<see cref="IFunction"/>的缓存配置转换为<see cref="DistributedCacheEntryOptions"/>
    /// </summary>
    public static DistributedCacheEntryOptions ToCacheOptions(this IFunction function)
    {
        Check.NotNull(function, nameof(function));
        if (function.CacheExpirationSeconds == 0)
        {
            return null;
        }
        DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
        if (!function.IsCacheSliding)
        {
            options.SetAbsoluteExpiration(TimeSpan.FromSeconds(function.CacheExpirationSeconds));
        }
        else
        {
            options.SetSlidingExpiration(TimeSpan.FromSeconds(function.CacheExpirationSeconds));
        }
        return options;
    }
        
}