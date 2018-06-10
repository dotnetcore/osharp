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

using OSharp.Caching;
using OSharp.Core.Functions;
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
        where TFunction : class, IFunction, IEntity<Guid>
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
        private readonly IDistributedCache _cache;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化一个<see cref="FunctionAuthCacheBase"/>类型的新实例
        /// </summary>
        protected FunctionAuthCacheBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cache = serviceProvider.GetService<IDistributedCache>();
        }

        /// <summary>
        /// 创建功能权限缓存，只创建 功能-角色集合 的映射，用户-功能 的映射，遇到才即时创建并缓存
        /// </summary>
        public virtual void BuildRoleCaches()
        {
            TFunction[] functions;
            //只创建 功能-角色集合 的映射，用户-功能 的映射，遇到才即时创建并缓存
            using (var scope = _serviceProvider.CreateScope())
            {
                IServiceProvider scopedProvider = scope.ServiceProvider;
                IRepository<TFunction, Guid> functionRepository = scopedProvider.GetService<IRepository<TFunction, Guid>>();
                functions = functionRepository.Entities.ToArray();
            }
            foreach (TFunction function in functions)
            {
                GetFunctionRoles(function.Id);
            }
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
            }
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
                return roleNames;
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                IServiceProvider scopedProvider = scope.ServiceProvider;
                IRepository<TModuleFunction, Guid> moduleFunctionRepository = scopedProvider.GetService<IRepository<TModuleFunction, Guid>>();
                TModuleKey[] moduleIds = moduleFunctionRepository.Entities.Where(m => m.FunctionId.Equals(functionId)).Select(m => m.ModuleId).Distinct()
                    .ToArray();
                IRepository<TModuleRole, Guid> moduleRoleRepository = scopedProvider.GetService<IRepository<TModuleRole, Guid>>();
                TRoleKey[] roleIds = moduleRoleRepository.Entities.Where(m => moduleIds.Contains(m.ModuleId)).Select(m => m.RoleId).Distinct().ToArray();
                IRepository<TRole, TRoleKey> roleRepository = scopedProvider.GetService<IRepository<TRole, TRoleKey>>();
                roleNames = roleRepository.Entities.Where(m => roleIds.Contains(m.Id)).Select(m => m.Name).Distinct().ToArray();
            }
            if (roleNames.Length > 0)
            {
                _cache.Set(key, roleNames);
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
                return functionIds;
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                IServiceProvider scopedProvider = scope.ServiceProvider;
                IRepository<TUser, TUserKey> userRepository = scopedProvider.GetService<IRepository<TUser, TUserKey>>();
                TUserKey userId = userRepository.Entities.Where(m => m.UserName == userName).Select(m => m.Id).FirstOrDefault();
                if (userId.Equals(default(TUserKey)))
                {
                    return new Guid[0];
                }
                IRepository<TModuleUser, Guid> moduleUserRepository = scopedProvider.GetService<IRepository<TModuleUser, Guid>>();
                TModuleKey[] moduleIds = moduleUserRepository.Entities.Where(m => m.UserId.Equals(userId)).Select(m => m.ModuleId).Distinct().ToArray();
                IRepository<TModule, TModuleKey> moduleRepository = scopedProvider.GetService<IRepository<TModule, TModuleKey>>();
                moduleIds = moduleIds.Select(m => moduleRepository.Entities.Where(n => n.TreePathString.Contains("$" + m + "$"))
                    .Select(n => n.Id)).SelectMany(m => m).Distinct().ToArray();
                IRepository<TModuleFunction, Guid> moduleFunctionRepository = scopedProvider.GetService<IRepository<TModuleFunction, Guid>>();
                functionIds = moduleFunctionRepository.Entities.Where(m => moduleIds.Contains(m.ModuleId)).Select(m => m.FunctionId).Distinct().ToArray();
            }
            if (functionIds.Length > 0)
            {
                _cache.Set(key, functionIds);
            }
            return functionIds;
        }
    }
}