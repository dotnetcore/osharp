// -----------------------------------------------------------------------
//  <copyright file="RoleSeedDataInitializer.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-06 21:56</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;

using OSharp.Hosting.Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Entity;
using OSharp.Exceptions;
using OSharp.Identity;


namespace OSharp.Hosting.Identity
{
    public class RoleSeedDataInitializer : SeedDataInitializerBase<Role, int>
    {
        private readonly IServiceProvider _rootProvider;

        /// <summary>
        /// 初始化一个<see cref="SeedDataInitializerBase{TEntity, TKey}"/>类型的新实例
        /// </summary>
        public RoleSeedDataInitializer(IServiceProvider rootProvider)
            : base(rootProvider)
        {
            _rootProvider = rootProvider;
        }

        /// <summary>
        /// 重写以提供要初始化的种子数据
        /// </summary>
        /// <returns></returns>
        protected override Role[] SeedData()
        {
            return new[]
            {
                new Role() { Name = "系统管理员", Remark = "系统最高权限管理角色", IsAdmin = true, IsSystem = true }
            };
        }

        /// <summary>
        /// 重写以提供判断某个实体是否存在的表达式
        /// </summary>
        /// <param name="entity">要判断的实体</param>
        /// <returns></returns>
        protected override Expression<Func<Role, bool>> ExistingExpression(Role entity)
        {
            return m => m.Name == entity.Name;
        }

        /// <summary>
        /// 将种子数据初始化到数据库
        /// </summary>
        /// <param name="entities"></param>
        protected override void SyncToDatabase(Role[] entities)
        {
            if (entities?.Length > 0)
            {
                _rootProvider.BeginUnitOfWorkTransaction(provider =>
                    {
                        RoleManager<Role> roleManager = provider.GetService<RoleManager<Role>>();
                        foreach (Role role in entities)
                        {
                            if (roleManager.Roles.Any(ExistingExpression(role)))
                            {
                                continue;
                            }
                            IdentityResult result = roleManager.CreateAsync(role).Result;
                            if (!result.Succeeded)
                            {
                                throw new OsharpException($"进行角色种子数据“{role.Name}”同步时出错：{result.ErrorMessage()}");
                            }
                        }
                    });
            }
        }
    }
}


