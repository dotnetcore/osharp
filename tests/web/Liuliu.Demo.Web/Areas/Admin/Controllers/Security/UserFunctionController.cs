// -----------------------------------------------------------------------
//  <copyright file="UserFunctionController.cs" company="OSharp开源团队">
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

using Liuliu.Demo.Identity.Dtos;
using Liuliu.Demo.Identity.Entities;
using Liuliu.Demo.Security;
using Liuliu.Demo.Security.Dtos;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Linq;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 4, Position = "Security", PositionName = "权限安全模块")]
    [Description("管理-用户功能")]
    public class UserFunctionController : AdminApiController
    {
        private readonly IFilterService _filterService;
        private readonly SecurityManager _securityManager;
        private readonly UserManager<User> _userManager;

        public UserFunctionController(SecurityManager securityManager,
            UserManager<User> userManager, 
            RoleManager<Role> roleManager,
            IFilterService filterService)
        {
            _securityManager = securityManager;
            _userManager = userManager;
            _filterService = filterService;
        }

        /// <summary>
        /// 读取用户信息
        /// </summary>
        /// <returns>用户信息</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public PageData<UserOutputDto2> Read(PageRequest request)
        {
            request.FilterGroup.Rules.Add(new FilterRule("IsLocked", false, FilterOperate.Equal));
            Expression<Func<User, bool>> predicate = _filterService.GetExpression<User>(request.FilterGroup);
            var page = _userManager.Users.ToPage<User, UserOutputDto2>(predicate, request.PageCondition);
            return page.ToPageData();
        }

        /// <summary>
        /// 读取用户功能信息
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>用户功能信息</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [Description("读取功能")]
        public PageData<FunctionOutputDto2> ReadFunctions(int userId)
        {
            if (userId == 0)
            {
                return new PageData<FunctionOutputDto2>();
            }

            int[] moduleIds = _securityManager.GetUserWithRoleModuleIds(userId);
            Guid[] functionIds = _securityManager.ModuleFunctions.Where(m => moduleIds.Contains(m.ModuleId)).Select(m => m.FunctionId).Distinct()
                .ToArray();
            if (functionIds.Length == 0)
            {
                return new PageData<FunctionOutputDto2>();
            }

            PageRequest request = new PageRequest();
            Expression<Func<Function, bool>> funcExp = _filterService.GetExpression<Function>(request.FilterGroup);
            funcExp = funcExp.And(m => functionIds.Contains(m.Id));
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[] { new SortCondition("Area"), new SortCondition("Controller") };
            }

            PageResult<FunctionOutputDto2> page = _securityManager.Functions.ToPage<Function, FunctionOutputDto2>(funcExp, request.PageCondition);
            return page.ToPageData();
        }
    }
}