// -----------------------------------------------------------------------
//  <copyright file="RoleFunctionController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:49</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

using Liuliu.Demo.Authorization;
using Liuliu.Demo.Authorization.Dtos;
using Liuliu.Demo.Identity.Dtos;
using Liuliu.Demo.Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OSharp.Authorization.Functions;
using OSharp.Authorization.Modules;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Linq;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 3, Position = "Auth", PositionName = "权限授权模块")]
    [Description("管理-角色功能")]
    public class RoleFunctionController : AdminApiController
    {
        private readonly FunctionAuthManager _functionAuthManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IFilterService _filterService;

        public RoleFunctionController(FunctionAuthManager functionAuthManager, 
            RoleManager<Role> roleManager,
            IFilterService filterService)
        {
            _functionAuthManager = functionAuthManager;
            _roleManager = roleManager;
            _filterService = filterService;
        }

        /// <summary>
        /// 读取角色信息
        /// </summary>
        /// <returns>角色信息</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public PageData<RoleOutputDto2> Read(PageRequest request)
        {
            request.FilterGroup.Rules.Add(new FilterRule("IsLocked", false, FilterOperate.Equal));
            Expression<Func<Role, bool>> predicate = _filterService.GetExpression<Role>(request.FilterGroup);
            PageResult<RoleOutputDto2> page = _roleManager.Roles.ToPage<Role, RoleOutputDto2>(predicate, request.PageCondition);
            return page.ToPageData();
        }

        /// <summary>
        /// 读取角色功能信息
        /// </summary>
        /// <returns>角色功能信息</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [Description("读取功能")]
        public PageData<FunctionOutputDto2> ReadFunctions(int roleId, [FromBody]PageRequest request)
        {
            if (roleId == 0)
            {
                return new PageData<FunctionOutputDto2>();
            }
            int[] moduleIds = _functionAuthManager.GetRoleModuleIds(roleId);
            Guid[] functionIds = _functionAuthManager.ModuleFunctions.Where(m => moduleIds.Contains(m.ModuleId)).Select(m => m.FunctionId).Distinct()
                .ToArray();
            if (functionIds.Length == 0)
            {
                return new PageData<FunctionOutputDto2>();
            }

            Expression<Func<Function, bool>> funcExp = _filterService.GetExpression<Function>(request.FilterGroup);
            funcExp = funcExp.And(m => functionIds.Contains(m.Id));
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[] { new SortCondition("Area"), new SortCondition("Controller") };
            }

            var page = _functionAuthManager.Functions.ToPage<Function, FunctionOutputDto2>(funcExp, request.PageCondition);
            return page.ToPageData();
        }
    }
}