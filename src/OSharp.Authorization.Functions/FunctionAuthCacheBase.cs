// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthCacheBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-11 0:59</last-date>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;


namespace OSharp.Authorization;

/// <summary>
/// 功能权限配置缓存基类
/// </summary>
public abstract class FunctionAuthCacheBase<TModuleFunction, TModuleRole, TModuleUser, TFunction, TModule, TModuleKey,
        TRole, TRoleKey, TUser, TUserKey>
    : IFunctionAuthCache
    where TFunction : class, IFunction
    where TModule : ModuleBase<TModuleKey>
    where TModuleFunction : ModuleFunctionBase<TModuleKey>
    where TModuleKey : struct, IEquatable<TModuleKey>
    where TModuleRole : ModuleRoleBase<TModuleKey, TRoleKey>
    where TModuleUser : ModuleUserBase<TModuleKey, TUserKey>
    where TRole : RoleBase<TRoleKey>
    where TRoleKey : IEquatable<TRoleKey>
    where TUser : UserBase<TUserKey>
    where TUserKey : IEquatable<TUserKey>
{
    private readonly Random _random = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly IDistributedCache _cache;
    private readonly ILogger _logger;

    /// <summary>
    /// 初始化一个<see cref="FunctionAuthCacheBase{TModuleFunction, TModuleRole, TModuleUser, TFunction, TModule, TModuleKey,TRole, TRoleKey, TUser, TUserKey}"/>类型的新实例
    /// </summary>
    protected FunctionAuthCacheBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _cache = serviceProvider.GetService<IDistributedCache>();
        _logger = serviceProvider.GetLogger(GetType());
    }

    /// <summary>
    /// 创建功能权限缓存，只创建 功能-角色集合 的映射，用户-功能 的映射，遇到才即时创建并缓存
    /// </summary>
    public virtual void BuildRoleCaches()
    {
        //只创建 功能-角色集合 的映射，用户-功能 的映射，遇到才即时创建并缓存
        _serviceProvider.ExecuteScopedWork(provider =>
        {
            IRepository<TFunction, long> functionRepository = provider.GetRequiredService<IRepository<TFunction, long>>();
            long[] functionIds = functionRepository.QueryAsNoTracking(null, false).Select(m => m.Id).ToArray();

            foreach (long functionId in functionIds)
            {
                GetFunctionRoles(functionId, provider, true);
            }
            _logger.LogInformation($"功能权限：创建 {functionIds.Length} 个功能的“Function-Roles[]”缓存");
        });
    }

    /// <summary>
    /// 移除指定功能的缓存
    /// </summary>
    /// <param name="functionIds">功能编号集合</param>
    public virtual void RemoveFunctionCaches(params long[] functionIds)
    {
        foreach (long functionId in functionIds)
        {
            string key = GetFunctionRolesKey(functionId);
            _cache.Remove(key);
            _logger.LogDebug($"移除功能“{functionId}”的“Function-Roles[]”缓存");
        }
        _logger.LogInformation($"功能权限：移除{functionIds.Length}个“Function-Roles[]”缓存");
    }

    /// <summary>
    /// 移除指定用户的缓存
    /// </summary>
    /// <param name="userNames">用户编号集合</param>
    public virtual void RemoveUserCaches(params string[] userNames)
    {
        foreach (string userName in userNames)
        {
            string key = GetUserFunctionsKey(userName);
            _cache.Remove(key);
        }
    }

    /// <summary>
    /// 获取能执行指定功能的所有角色
    /// </summary>
    /// <param name="functionId">功能编号</param>
    /// <param name="scopeProvider">局部服务提供者</param>
    /// <param name="forceRefresh">是否强制刷新</param>
    /// <returns>能执行功能的角色名称集合</returns>
    public string[] GetFunctionRoles(long functionId, IServiceProvider scopeProvider = null, bool forceRefresh = false)
    {
        string key = GetFunctionRolesKey(functionId);
        IFunctionHandler functionHandler = _serviceProvider.GetRequiredService<IFunctionHandler>();
        var function = functionHandler.GetFunction(functionId);
        if (function == null)
        {
            return Array.Empty<string>();
        }
        string[] roleNames;
        if (!forceRefresh)
        {
            roleNames = _cache.Get<string[]>(key);
            if (roleNames != null)
            {
                _logger.LogDebug($"从缓存中获取到功能“{function.Name}”的“Function-Roles[]”缓存，角色数：{roleNames.Length}");
                return roleNames;
            }
        }

        IServiceProvider provider = scopeProvider;
        IServiceScope serviceScope = null;
        if (provider == null)
        {
            serviceScope = _serviceProvider.CreateScope();
            provider = serviceScope.ServiceProvider;
        }

        IRepository<TModuleFunction, long> moduleFunctionRepository = provider.GetRequiredService<IRepository<TModuleFunction, long>>();
        TModuleKey[] moduleIds = moduleFunctionRepository.QueryAsNoTracking(m => m.FunctionId.Equals(functionId)).Select(m => m.ModuleId).Distinct()
            .ToArray();
        if (moduleIds.Length == 0)
        {
            serviceScope?.Dispose();
            return Array.Empty<string>();
        }

        roleNames = Array.Empty<string>();
        IRepository<TModuleRole, long> moduleRoleRepository = provider.GetRequiredService<IRepository<TModuleRole, long>>();
        TRoleKey[] roleIds = moduleRoleRepository.QueryAsNoTracking(m => moduleIds.Contains(m.ModuleId)).Select(m => m.RoleId).Distinct().ToArray();
        if (roleIds.Length > 0)
        {
            IRepository<TRole, TRoleKey> roleRepository = provider.GetRequiredService<IRepository<TRole, TRoleKey>>();
            roleNames = roleRepository.QueryAsNoTracking(m => roleIds.Contains(m.Id)).Select(m => m.Name).Distinct().ToArray();
        }

        // 有效期为 7 ± 1 天
        int seconds = 7 * 24 * 3600 + _random.Next(-24 * 3600, 24 * 3600);
        _cache.Set(key, roleNames, seconds);
        _logger.LogDebug($"添加功能“{function.Name}”的“Function-Roles[]”缓存，角色数：{roleNames.Length}");

        serviceScope?.Dispose();
        return roleNames;
    }

    /// <summary>
    /// 获取指定用户的所有特权功能
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns>用户的所有特权功能</returns>
    public virtual long[] GetUserFunctions(string userName)
    {
        string key = GetUserFunctionsKey(userName);
        long[] functionIds = _cache.Get<long[]>(key);
        if (functionIds != null)
        {
            _logger.LogDebug($"从缓存中获取到用户“{userName}”的“User-Function[]”缓存");
            return functionIds;
        }
        functionIds = _serviceProvider.ExecuteScopedWork(provider =>
        {
            IRepository<TUser, TUserKey> userRepository = provider.GetRequiredService<IRepository<TUser, TUserKey>>();
            TUserKey userId = userRepository.QueryAsNoTracking(m => m.UserName == userName).Select(m => m.Id).FirstOrDefault();
            if (Equals(userId, default(TUserKey)))
            {
                return Array.Empty<long>();
            }
            IRepository<TModuleUser, long> moduleUserRepository = provider.GetRequiredService<IRepository<TModuleUser, long>>();
            TModuleKey[] moduleIds = moduleUserRepository.QueryAsNoTracking(m => m.UserId.Equals(userId)).Select(m => m.ModuleId).Distinct().ToArray();
            IRepository<TModule, TModuleKey> moduleRepository = provider.GetRequiredService<IRepository<TModule, TModuleKey>>();
            moduleIds = moduleIds.Select(m => moduleRepository.QueryAsNoTracking(n => n.TreePathString.Contains("$" + m + "$"))
                .Select(n => n.Id)).SelectMany(m => m).Distinct().ToArray();
            IRepository<TModuleFunction, long> moduleFunctionRepository = provider.GetRequiredService<IRepository<TModuleFunction, long>>();
            return moduleFunctionRepository.QueryAsNoTracking(m => moduleIds.Contains(m.ModuleId)).Select(m => m.FunctionId).Distinct().ToArray();
        });

        // 有效期为 7 ± 1 天
        int seconds = 7 * 24 * 3600 + _random.Next(-24 * 3600, 24 * 3600);
        _cache.Set(key, functionIds, seconds);
        _logger.LogDebug($"创建用户“{userName}”的“User-Function[]”缓存");

        return functionIds;
    }

    private static string GetFunctionRolesKey(long functionId)
    {
        return $"Auth:Function:FunctionRoles:{functionId}";
    }

    private static string GetUserFunctionsKey(string userName)
    {
        return $"Auth:Function:UserFunctions:{userName}";
    }
}
