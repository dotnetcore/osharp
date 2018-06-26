// -----------------------------------------------------------------------
//  <copyright file="RoleFunctionController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-13 2:03</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.UI;
using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Demo.Identity.Dtos;
using OSharp.Demo.Identity.Entities;
using OSharp.Demo.Security;
using OSharp.Demo.Security.Dtos;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Linq;


namespace OSharp.Demo.WebApi.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 3, Position = "Security")]
    [Description("管理-角色功能")]
    public class RoleFunctionController : AdminApiController
    {
        private readonly SecurityManager _securityManager;
        private readonly RoleManager<Role> _roleManager;

        public RoleFunctionController(SecurityManager securityManager, RoleManager<Role> roleManager)
        {
            _securityManager = securityManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// 读取角色信息
        /// </summary>
        /// <returns>角色信息</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public PageData<RoleOutputDto2> Read()
        {
            PageRequest request = new PageRequest(Request);
            request.FilterGroup.Rules.Add(new FilterRule("IsLocked", false, FilterOperate.Equal));
            Expression<Func<Role, bool>> predicate = FilterHelper.GetExpression<Role>(request.FilterGroup);
            PageResult<RoleOutputDto2> page = _roleManager.Roles.ToPage<Role, RoleOutputDto2>(predicate, request.PageCondition);
            return page.ToPageData();
        }

        /// <summary>
        /// 读取角色功能信息
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns>角色功能信息</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [Description("读取功能")]
        public PageData<FunctionOutputDto2> ReadFunctions(int roleId)
        {
            if (roleId == 0)
            {
                return new PageData<FunctionOutputDto2>();
            }
            int[] moduleIds = _securityManager.GetRoleModuleIds(roleId);
            Guid[] functionIds = _securityManager.ModuleFunctions.Where(m => moduleIds.Contains(m.ModuleId)).Select(m => m.FunctionId).Distinct()
                .ToArray();
            if (functionIds.Length == 0)
            {
                return new PageData<FunctionOutputDto2>();
            }

            PageRequest request = new PageRequest(Request);
            Expression<Func<Function, bool>> funcExp = FilterHelper.GetExpression<Function>(request.FilterGroup);
            funcExp = funcExp.And(m => functionIds.Contains(m.Id));
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[] { new SortCondition("Area"), new SortCondition("Controller") };
            }

            var page = _securityManager.Functions.ToPage<Function, FunctionOutputDto2>(funcExp, request.PageCondition);
            return page.ToPageData();
        }
    }
}