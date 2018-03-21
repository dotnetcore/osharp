using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Modules;
using OSharp.Demo.Identity.Entities;
using OSharp.Demo.Security;
using OSharp.Demo.Security.Dtos;
using OSharp.Entity;


namespace OSharp.Demo.WebApi.Startups
{
    public class SeedDataInitModule : OSharpModule
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override ModuleLevel Level => ModuleLevel.Application;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 2;

        /// <summary>
        /// 使用模块服务
        /// </summary>
        /// <param name="provider">根服务提供者</param>
        public override void UseModule(IServiceProvider provider)
        {
            using (IServiceScope scope = provider.CreateScope())
            {
                IUnitOfWork uow = scope.ServiceProvider.GetService<IUnitOfWork>();

                RoleManager<Role> roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();
                if (roleManager.Roles.Any())
                {
                    return;
                }

                Role role = new Role() { Name = "系统管理员", Remark = "系统最高权限管理角色", IsAdmin = true, IsDefault = false, IsSystem = true };
                roleManager.CreateAsync(role).Wait();

                User user = new User() { UserName = "admin", Email = "admin@osharp.org", EmailConfirmed = true, LockoutEnabled = true, IsSystem = true };
                UserManager<User> userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                userManager.CreateAsync(user).Wait();
                userManager.AddToRoleAsync(user, role.Name).Wait();

                ModuleInputDto moduleDto = new ModuleInputDto() { Name = "根节点", Remark = "系统根节点", OrderCode = 1 };
                SecurityManager securityManager = scope.ServiceProvider.GetService<SecurityManager>();
                securityManager.CreateModule(moduleDto).Wait();

                uow.Commit();
            }

            IsEnabled = true;
        }
    }
}
