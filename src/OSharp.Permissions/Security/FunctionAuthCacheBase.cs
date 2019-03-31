// -----------------------------------------------------------------------
//  <copyright file="FunctionAuthCacheBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-11 0:59</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Caching;
using OSharp.Core.Functions;
using OSharp.Dependency;
using OSharp.Entity;
using OSharp.Identity;
using OSharp.Secutiry;


namespace OSharp.Security
{
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
            TFunction[] functions = _serviceProvider.ExecuteScopedWork(provider =>
            {
                IRepository<TFunction, Guid> functionRepository = provider.GetService<IRepository<TFunction, Guid>>();
                return functionRepository.Query(null, false).ToArray();
            });

            foreach (TFunction function in functions)
            {
                GetFunctionRoles(function.Id);
            }
            _logger.LogInformation($"功能权限：创建{functions.Length}个功能的“Function-Roles[]”缓存");
        }

        /// <summary>
        /// 移除指定功能的缓存
        /// </summary>
        /// <param name="functionIds">功能编号集合</param>
        public virtual void RemoveFunctionCaches(params Guid[] functionIds)
        {
            foreach (Guid functionId in functionIds)
            {
                string key = $"Security_FunctionRoles_{functionId}";
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
                string key = $"Security_UserFunctions_{userName}";
                _cache.Remove(key);
            }
        }

        /// <summary>
        /// 获取能执行指定功能的所有角色
        /// </summary>
        /// <param name="functionId">功能编号</param>
        /// <returns>能执行功能的角色名称集合</returns>
        public virtual string[] GetFunctionRoles(Guid functionId)
        {
            string key = $"Security_FunctionRoles_{functionId}";
            string[] roleNames = _cache.Get<string[]>(key);
            if (roleNames != null)
            {
                _logger.LogDebug($"从缓存中获取到功能“{functionId}”的“Function-Roles[]”缓存");
                return roleNames;
            }
            roleNames = _serviceProvider.ExecuteScopedWork(provider =>
            {
                IRepository<TModuleFunction, Guid> moduleFunctionRepository = provider.GetService<IRepository<TModuleFunction, Guid>>();
                TModuleKey[] moduleIds = moduleFunctionRepository.Query(m => m.FunctionId.Equals(functionId)).Select(m => m.ModuleId).Distinct()
                    .ToArray();
                if (moduleIds.Length == 0)
                {
                    return new string[0];
                }
                IRepository<TModuleRole, Guid> moduleRoleRepository = provider.GetService<IRepository<TModuleRole, Guid>>();
                TRoleKey[] roleIds = moduleRoleRepository.Query(m => moduleIds.Contains(m.ModuleId)).Select(m => m.RoleId).Distinct().ToArray();
                if (roleIds.Length == 0)
                {
                    return new string[0];
                }
                IRepository<TRole, TRoleKey> roleRepository = provider.GetService<IRepository<TRole, TRoleKey>>();
                return roleRepository.Query(m => roleIds.Contains(m.Id)).Select(m => m.Name).Distinct().ToArray();
            });

            if (roleNames != null)
            {
                _cache.Set(key, roleNames);
                _logger.LogDebug($"添加功能“{functionId}”的“Function-Roles[]”缓存");
            }
            return roleNames;
        }

        /// <summary>
        /// 获取指定用户的所有特权功能
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户的所有特权功能</returns>
        public virtual Guid[] GetUserFunctions(string userName)
        {
            string key = $"Security_UserFunctions_{userName}";
            Guid[] functionIds = _cache.Get<Guid[]>(key);
            if (functionIds != null)
            {
                _logger.LogDebug($"从缓存中获取到用户“{userName}”的“User-Function[]”缓存");
                return functionIds;
            }
            functionIds = _serviceProvider.ExecuteScopedWork(provider =>
            {
                IRepository<TUser, TUserKey> userRepository = provider.GetService<IRepository<TUser, TUserKey>>();
                TUserKey userId = userRepository.Query(m => m.UserName == userName).Select(m => m.Id).FirstOrDefault();
                if (Equals(userId, default(TUserKey)))
                {
                    return new Guid[0];
                }
                IRepository<TModuleUser, Guid> moduleUserRepository = provider.GetService<IRepository<TModuleUser, Guid>>();
                TModuleKey[] moduleIds = moduleUserRepository.Query(m => m.UserId.Equals(userId)).Select(m => m.ModuleId).Distinct().ToArray();
                IRepository<TModule, TModuleKey> moduleRepository = provider.GetService<IRepository<TModule, TModuleKey>>();
                moduleIds = moduleIds.Select(m => moduleRepository.Query(n => n.TreePathString.Contains("$" + m + "$"))
                    .Select(n => n.Id)).SelectMany(m => m).Distinct().ToArray();
                IRepository<TModuleFunction, Guid> moduleFunctionRepository = provider.GetService<IRepository<TModuleFunction, Guid>>();
                return moduleFunctionRepository.Query(m => moduleIds.Contains(m.ModuleId)).Select(m => m.FunctionId).Distinct().ToArray();
            });

            if (functionIds.Length > 0)
            {
                _logger.LogDebug($"创建用户“{userName}”的“User-Function[]”缓存");
                _cache.Set(key, functionIds);
            }
            return functionIds;
        }
    }
}