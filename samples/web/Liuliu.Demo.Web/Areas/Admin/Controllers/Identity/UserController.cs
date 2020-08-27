// -----------------------------------------------------------------------
//  <copyright file="UserController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:49</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Liuliu.Demo.Authorization;
using Liuliu.Demo.Authorization.Dtos;
using Liuliu.Demo.Common.Dtos;
using Liuliu.Demo.Identity;
using Liuliu.Demo.Identity.Dtos;
using Liuliu.Demo.Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Authorization;
using OSharp.Authorization.Functions;
using OSharp.Authorization.Modules;
using OSharp.Caching;
using OSharp.Collections;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Filter;
using OSharp.Identity;
using OSharp.Mapping;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 1, Position = "Identity", PositionName = "身份认证模块")]
    [Description("管理-用户信息")]
    public class UserController : AdminApiController
    {
        private readonly IIdentityContract _identityContract;
        private readonly ICacheService _cacheService;
        private readonly IFilterService _filterService;
        private readonly UserManager<User> _userManager;
        private readonly FunctionAuthManager _functionAuthManager;

        public UserController(
            UserManager<User> userManager,
            FunctionAuthManager functionAuthManager,
            IIdentityContract identityContract,
            ICacheService cacheService,
            IFilterService filterService)
        {
            _userManager = userManager;
            _functionAuthManager = functionAuthManager;
            _identityContract = identityContract;
            _cacheService = cacheService;
            _filterService = filterService;
        }

        /// <summary>
        /// 读取用户列表信息
        /// </summary>
        /// <returns>用户列表信息</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public PageData<UserOutputDto> Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));
            IFunction function = this.GetExecuteFunction();

            Func<User, bool> updateFunc = _filterService.GetDataFilterExpression<User>(null, DataAuthOperation.Update).Compile();
            Func<User, bool> deleteFunc = _filterService.GetDataFilterExpression<User>(null, DataAuthOperation.Delete).Compile();
            Expression<Func<User, bool>> predicate = _filterService.GetExpression<User>(request.FilterGroup);

            //查询某一角色的所有用户
            //var roleId = request.FilterGroup.Rules.FirstOrDefault(m => m.Field == "RoleId")?.CastTo(0);
            //if (roleId != 0)
            //{
            //    predicate = predicate.And(m => m.UserRoles.Any(n => n.RoleId == roleId));
            //}

            var page = _userManager.Users.ToPage(predicate, request.PageCondition, m => new
            {
                D = m,
                Roles = m.UserRoles.Select(n => n.Role.Name)
            }).ToPageResult(data => data.Select(m => new UserOutputDto(m.D)
            {
                Roles = m.Roles.ToArray(),
                Updatable = updateFunc(m.D),
                Deletable = deleteFunc(m.D)
            }).ToArray());
            
            return page.ToPageData();
        }

        /// <summary>
        /// 读取用户节点信息
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("读取节点")]
        public ListNode[] ReadNode(FilterGroup group)
        {
            Check.NotNull(group, nameof(group));
            IFunction function = this.GetExecuteFunction();
            Expression<Func<User, bool>> exp = _filterService.GetExpression<User>(group);
            ListNode[] nodes = _cacheService.ToCacheArray<User, ListNode>(_userManager.Users, exp, m => new ListNode()
            {
                Id = m.Id,
                Name = m.NickName
            }, function);
            return nodes;
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="dtos">用户信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [UnitOfWork]
        [Description("新增")]
        public async Task<AjaxResult> Create(UserInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            List<string> names = new List<string>();
            foreach (var dto in dtos)
            {
                User user = dto.MapTo<User>();
                IdentityResult result = dto.Password.IsMissing()
                    ? await _userManager.CreateAsync(user)
                    : await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    return result.ToOperationResult().ToAjaxResult();
                }
                names.Add(user.UserName);
            }
            return new AjaxResult($"用户“{names.ExpandAndToString()}”创建成功");
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="dtos">用户信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [UnitOfWork]
        [Description("更新")]
        public async Task<AjaxResult> Update(UserInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            List<string> names = new List<string>();
            foreach (var dto in dtos)
            {
                User user = await _userManager.FindByIdAsync(dto.Id.ToString());
                user = dto.MapTo(user);
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return result.ToOperationResult().ToAjaxResult();
                }
                names.Add(user.UserName);
            }
            return new AjaxResult($"用户“{names.ExpandAndToString()}”更新成功");
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="ids">用户信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [UnitOfWork]
        [Description("删除")]
        public async Task<AjaxResult> Delete(int[] ids)
        {
            Check.NotNull(ids, nameof(ids));
            List<string> names = new List<string>();
            foreach (int id in ids)
            {
                User user = await _userManager.FindByIdAsync(id.ToString());
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return result.ToOperationResult().ToAjaxResult();
                }
                names.Add(user.UserName);
            }
            return new AjaxResult($"用户“{names.ExpandAndToString()}”删除成功");
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="dto">用户角色信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [DependOnFunction("ReadUserRoles", Controller = "Role")]
        [UnitOfWork]
        [Description("设置角色")]
        public async Task<AjaxResult> SetRoles(UserSetRoleDto dto)
        {
            OperationResult result = await _identityContract.SetUserRoles(dto.UserId, dto.RoleIds);
            return result.ToAjaxResult();
        }

        /// <summary>
        /// 设置用户模块
        /// </summary>
        /// <param name="dto">用户模块信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [DependOnFunction("ReadUserModules", Controller = "Module")]
        [UnitOfWork]
        [Description("设置模块")]
        public async Task<AjaxResult> SetModules(UserSetModuleDto dto)
        {
            OperationResult result = await _functionAuthManager.SetUserModules(dto.UserId, dto.ModuleIds);
            return result.ToAjaxResult();
        }
    }
}