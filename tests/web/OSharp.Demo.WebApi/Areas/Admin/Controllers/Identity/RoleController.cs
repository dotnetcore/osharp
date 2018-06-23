// -----------------------------------------------------------------------
//  <copyright file="RoleController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-10 16:38</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Collections;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Demo.Common.Dtos;
using OSharp.Demo.Identity;
using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;
using OSharp.Demo.Security;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Identity;
using OSharp.Mapping;
using OSharp.Security;


namespace OSharp.Demo.WebApi.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 2, Position = "Identity")]
    [Description("管理-角色信息")]
    public class RoleController : AdminApiController
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly SecurityManager _securityManager;
        private readonly IIdentityContract _identityContract;

        public RoleController(RoleManager<Role> roleManager,
            SecurityManager securityManager,
            IIdentityContract identityContract)
        {
            _roleManager = roleManager;
            _securityManager = securityManager;
            _identityContract = identityContract;
        }

        [ModuleInfo]
        [Description("读取")]
        public IActionResult Read()
        {
            PageRequest request = new PageRequest(Request);
            Expression<Func<Role, bool>> predicate = FilterHelper.GetExpression<Role>(request.FilterGroup);
            var page = _roleManager.Roles.ToPage(predicate, request.PageCondition, m => new
            {
                m.Id,
                m.Name,
                m.Remark,
                m.IsAdmin,
                m.IsDefault,
                m.IsLocked,
                m.CreatedTime
            });

            return Json(page.ToPageData());
        }

        [Description("读取节点")]
        public IActionResult ReadNode()
        {
            var nodes = _roleManager.Roles.Unlocked().OrderBy(m => m.Name).Select(m => new
            {
                RoleId = m.Id,
                RoleName = m.Name
            });
            return Json(nodes);
        }

        [Description("读取角色[用户]树数据")]
        public IActionResult ReadUserRoles(int userId)
        {
            Check.GreaterThan(userId, nameof(userId), 0);

            int[] checkRoleIds = _identityContract.UserRoles.Where(m => m.UserId == userId).Select(m => m.RoleId).Distinct().ToArray();
            var nodes = _identityContract.Roles.OrderByDescending(m => m.IsAdmin).ThenBy(m => m.Id).Select(m => new
            {
                m.Id,
                m.Name,
                IsChecked = checkRoleIds.Contains(m.Id)
            }).ToList();
            return Json(nodes);
        }

        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("新增")]
        public async Task<IActionResult> Create(RoleInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            List<string> names = new List<string>();
            foreach (RoleInputDto dto in dtos)
            {
                Role role = dto.MapTo<Role>();
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    return Json(result.ToOperationResult().ToAjaxResult());
                }
                names.Add(role.Name);
            }
            return Json(new AjaxResult($"角色“{names.ExpandAndToString()}”创建成功"));
        }

        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新")]
        public async Task<IActionResult> Update(RoleInputDto[] dtos)
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
                    return Json(result.ToOperationResult().ToAjaxResult());
                }
                names.Add(role.Name);
            }
            return Json(new AjaxResult($"角色“{names.ExpandAndToString()}”更新成功"));
        }

        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除")]
        public async Task<IActionResult> Delete(int[] ids)
        {
            Check.NotNull(ids, nameof(ids));
            List<string> names = new List<string>();
            foreach (int id in ids)
            {
                Role role = await _roleManager.FindByIdAsync(id.ToString());
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    return Json(result.ToOperationResult().ToAjaxResult());
                }
                names.Add(role.Name);
            }
            return Json(new AjaxResult($"角色“{names.ExpandAndToString()}”删除成功"));
        }

        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [DependOnFunction("ReadRoleModules", Controller = "Module")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("权限设置")]
        public async Task<ActionResult> SetPermission([FromBody]RoleSetPermissionDto dto)
        {
            OperationResult result = await _securityManager.SetRoleModules(dto.RoleId, dto.ModuleIds);
            return Json(result.ToAjaxResult());
        }
    }
}