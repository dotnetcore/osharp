// -----------------------------------------------------------------------
//  <copyright file="IdentityService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-18 14:48</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using OSharp.Collections;
using OSharp.Data;
using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;
using OSharp.Demo.Identity.Events;
using OSharp.Entity;
using OSharp.EventBuses;
using OSharp.Identity;


namespace OSharp.Demo.Identity
{
    /// <summary>
    /// 业务实现：身份认证模块
    /// </summary>
    public partial class IdentityService : IIdentityContract
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IEventBus _eventBus;
        private readonly IRepository<UserRole, Guid> _userRoleRepository;

        /// <summary>
        /// 初始化一个<see cref="IdentityService"/>类型的新实例
        /// </summary>
        public IdentityService(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IEventBus eventBus,
            IRepository<UserRole, Guid> userRoleRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _eventBus = eventBus;
            _userRoleRepository = userRoleRepository;
        }

        /// <summary>
        /// 获取 角色信息查询数据集
        /// </summary>
        public IQueryable<Role> Roles
        {
            get { return _roleManager.Roles; }
        }

        /// <summary>
        /// 获取 用户信息查询数据集
        /// </summary>
        public IQueryable<User> Users
        {
            get { return _userManager.Users; }
        }

        /// <summary>
        /// 使用账号登录
        /// </summary>
        /// <param name="dto">登录信息</param>
        /// <returns>业务操作结果</returns>
        public async Task<OperationResult<User>> Login(LoginDto dto)
        {
            User user = await FindUserByAccount(dto.Account);
            if (user == null)
            {
                return new OperationResult<User>(OperationResultType.Error, "用户不存在");
            }
            if (user.IsLocked)
            {
                return new OperationResult<User>(OperationResultType.Error, "用户已被冻结，无法登录");
            }
            if (await _userManager.IsLockedOutAsync(user))
            {
                return new OperationResult<User>(OperationResultType.Error, $"用户因密码错误次数过多而被锁定 {_userManager.Options.Lockout.DefaultLockoutTimeSpan.Minutes} 分钟，请稍后重试");
            }
            if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                if (await _userManager.GetLockoutEnabledAsync(user))
                {
                    await _userManager.AccessFailedAsync(user);
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        return new OperationResult<User>(OperationResultType.Error, $"用户因密码错误次数过多而被锁定 {_userManager.Options.Lockout.DefaultLockoutTimeSpan.Minutes} 分钟，请稍后重试");
                    }
                    return new OperationResult<User>(OperationResultType.Error, $"用户名或密码错误，您还有 {_userManager.Options.Lockout.MaxFailedAccessAttempts - await _userManager.GetAccessFailedCountAsync(user)} 次机会");
                }
                return new OperationResult<User>(OperationResultType.Error, "用户名或密码错误");
            }
            //触发登录成功事件
            LoginEventData loginEventData = new LoginEventData() { LoginDto = dto, User = user };
            _eventBus.PublishSync(loginEventData);

            return new OperationResult<User>(OperationResultType.Success, "用户登录成功", user);
        }

        /// <summary>
        /// 依次按用户名，Email，手机查找用户
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        private async Task<User> FindUserByAccount(string account)
        {
            User user = await _userManager.FindByNameAsync(account);
            if (user != null)
            {
                return user;
            }
            if (account.IsEmail())
            {
                user = await _userManager.FindByEmailAsync(account);
                if (user != null)
                {
                    return user;
                }
            }
            if (account.IsMobileNumber())
            {
                user = _userManager.Users.FirstOrDefault(m => m.PhoneNumber == account);
            }
            return user;
        }
    }
}