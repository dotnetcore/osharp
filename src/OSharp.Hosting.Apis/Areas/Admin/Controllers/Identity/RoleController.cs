// -----------------------------------------------------------------------
//  <copyright file="RoleController.cs" company="OSharp开源团队">
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

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Authorization.Functions;
using OSharp.Authorization.Modules;
using OSharp.Caching;
using OSharp.Collections;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Hosting.Authorization;
using OSharp.Hosting.Authorization.Dtos;
using OSharp.Hosting.Identity;
using OSharp.Hosting.Identity.Dtos;
using OSharp.Hosting.Identity.Entities;
using OSharp.Identity;
using OSharp.Mapping;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 2, Position = "Identity", PositionName = "身份认证模块")]
    [Description("管理-角色信息")]
    public class RoleController : AdminApiControllerBase
    {
        private readonly IIdentityContract _identityContract;
        private readonly FunctionAuthManager _functionAuthorizationManager;
        private readonly ICacheService _cacheService;
        private readonly IFilterService _filterService;
        private readonly RoleManager<Role> _roleManager;

        public RoleController(RoleManager<Role> roleManager,
            IIdentityContract identityContract,
            FunctionAuthManager functionAuthorizationManager,
            ICacheService cacheService,
            IFilterService filterService)
        {
            _roleManager = roleManager;
            _identityContract = identityContract;
            _functionAuthorizationManager = functionAuthorizationManager;
            _cacheService = cacheService;
            _filterService = filterService;
        }

        /// <summary>
        /// 读取角色
        /// </summary>
        /// <returns>角色页列表</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public AjaxResult Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));
            IFunction function = this.GetExecuteFunction();

            Expression<Func<Role, bool>> predicate = _filterService.GetExpression<Role>(request.FilterGroup);
            var page = _cacheService.ToPageCache<Role, RoleOutputDto>(_roleManager.Roles, predicate, request.PageCondition, function);

            return new AjaxResult("数据读取成功", AjaxResultType.Success, page.ToPageData());
        }

        /// <summary>
        /// 读取角色节点
        /// </summary>
        /// <returns>角色节点列表</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("读取节点")]
        public RoleNode[] ReadNode()
        {
            IFunction function = this.GetExecuteFunction();
            Expression<Func<Role, bool>> exp = m => !m.IsLocked;

            RoleNode[] nodes = _cacheService.ToCacheArray(_roleManager.Roles, exp, m => new RoleNode()
            {
                RoleId = m.Id,
                RoleName = m.Name
            }, function);
            return nodes;
        }

        /// <summary>
        /// 读取角色[用户]树数据
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>角色[用户]树数据</returns>
        [HttpGet]
        [Description("读取角色[用户]树数据")]
        public List<UserRoleNode> ReadUserRoles(int userId)
        {
            Check.GreaterThan(userId, nameof(userId), 0);

            int[] checkRoleIds = _identityContract.UserRoles.Where(m => m.UserId == userId).Select(m => m.RoleId).Distinct().ToArray();
            List<UserRoleNode> nodes = _identityContract.Roles.Where(m => !m.IsLocked)
                .OrderByDescending(m => m.IsAdmin).ThenBy(m => m.Id).ToOutput<Role, UserRoleNode>().ToList();
            nodes.ForEach(m => m.IsChecked = checkRoleIds.Contains(m.Id));
            return nodes;
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="dtos">新增角色信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction(nameof(Read))]
        [UnitOfWork]
        [Description("新增")]
        public async Task<AjaxResult> Create(RoleInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            List<string> names = new List<string>();
            foreach (RoleInputDto dto in dtos)
            {
                Role role = dto.MapTo<Role>();
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    return result.ToOperationResult().ToAjaxResult();
                }
                names.Add(role.Name);
            }
            return new AjaxResult($"角色“{names.ExpandAndToString()}”创建成功");
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="dtos">更新角色信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction(nameof(Read))]
        [UnitOfWork]
        [Description("更新")]
        public async Task<AjaxResult> Update(RoleInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            List<string> names = new List<string>();
            foreach (RoleInputDto dto in dtos)
            {
                Role role = await _roleManager.FindByIdAsync(dto.Id.ToString());
                role = dto.MapTo(role);
                IdentityResult result = await _roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    return result.ToOperationResult().ToAjaxResult();
                }
                names.Add(role.Name);
            }
            return new AjaxResult($"角色“{names.ExpandAndToString()}”更新成功");
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids">要删除的角色编号</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction(nameof(Read))]
        [UnitOfWork]
        [Description("删除")]
        public async Task<AjaxResult> Delete(int[] ids)
        {
            Check.NotNull(ids, nameof(ids));
            List<string> names = new List<string>();
            foreach (int id in ids)
            {
                Role role = await _roleManager.FindByIdAsync(id.ToString());
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    return result.ToOperationResult().ToAjaxResult();
                }
                names.Add(role.Name);
            }
            return new AjaxResult($"角色“{names.ExpandAndToString()}”删除成功");
        }

        /// <summary>
        /// 设置角色模块
        /// </summary>
        /// <param name="dto">角色模块信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction(nameof(Read))]
        [DependOnFunction(nameof(ModuleController.ReadRoleModules), Controller = nameof(ModuleController))]
        [UnitOfWork]
        [Description("设置模块")]
        public async Task<AjaxResult> SetModules(RoleSetModuleDto dto)
        {
            OperationResult result = await _functionAuthorizationManager.SetRoleModules(dto.RoleId, dto.ModuleIds);
            return result.ToAjaxResult();
        }
    }
}
