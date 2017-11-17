// -----------------------------------------------------------------------
//  <copyright file="IdentityServiceCollectionExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-12 0:00</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Identity;

using OSharp.Identity;


namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Identity服务注册扩展方法
    /// </summary>
    public static class IdentityServiceCollectionExtensions
    {
        /// <summary>
        /// 向<see cref="IServiceCollection"/>中添加Identity服务
        /// </summary>
        /// <typeparam name="TUserStore">用户存储</typeparam>
        /// <typeparam name="TRoleStore">角色存储</typeparam>
        /// <typeparam name="TUser">用户类型</typeparam>
        /// <typeparam name="TRole">角色类型</typeparam>
        /// <typeparam name="TUserKey">用户编号类型</typeparam>
        /// <typeparam name="TRoleKey">角色编号类型</typeparam>
        /// <param name="services">服务信任</param>
        /// <param name="setupAction">选项</param>
        /// <returns></returns>
        public static IdentityBuilder AddOSharpIdentity<TUserStore, TRoleStore, TUser, TRole, TUserKey, TRoleKey>(this IServiceCollection services, Action<IdentityOptions>setupAction = null)
            where TUserStore : class, IUserStore<TUser>
            where TRoleStore : class, IRoleStore<TRole>
            where TUser : UserBase<TUserKey>
            where TRole : RoleBase<TRoleKey>
            where TUserKey : IEquatable<TUserKey>
            where TRoleKey : IEquatable<TRoleKey>
        {
            services.AddScoped<IUserStore<TUser>, TUserStore>();
            services.AddScoped<IRoleStore<TRole>, TRoleStore>();

            return services.AddIdentity<TUser, TRole>(setupAction);
        }
    }
}