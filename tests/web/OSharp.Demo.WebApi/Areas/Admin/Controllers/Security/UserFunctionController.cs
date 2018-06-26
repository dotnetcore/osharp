// -----------------------------------------------------------------------
//  <copyright file="UserFunctionController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-13 2:09</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.UI;
using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Demo.Identity.Entities;
using OSharp.Demo.Security;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Linq;
using OSharp.Security;


namespace OSharp.Demo.WebApi.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 4, Position = "Security")]
    [Description("管理-用户功能")]
    public class UserFunctionController : AdminApiController
    {
        private readonly SecurityManager _securityManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserFunctionController(SecurityManager securityManager, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _securityManager = securityManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [ModuleInfo]
        [Description("读取")]
        public IActionResult Read()
        {
            PageRequest request = new PageRequest(Request);
            request.FilterGroup.Rules.Add(new FilterRule("IsLocked", false, FilterOperate.Equal));
            Expression<Func<User, bool>> predicate = FilterHelper.GetExpression<User>(request.FilterGroup);
            var page = _userManager.Users.ToPage(predicate,
                request.PageCondition,
                m => new
                {
                    m.Id,
                    m.UserName,
                    m.Email
                });
            return Json(page.ToPageData());
        }

        [HttpGet]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [Description("读取功能")]
        public IActionResult ReadFunctions(int userId)
        {
            if (userId == 0)
            {
                return Json(new PageData<object>());
            }

            int[] moduleIds = _securityManager.GetUserWithRoleModuleIds(userId);
            Guid[] functionIds = _securityManager.ModuleFunctions.Where(m => moduleIds.Contains(m.ModuleId)).Select(m => m.FunctionId).Distinct()
                .ToArray();
            if (functionIds.Length == 0)
            {
                return Json(new PageData<object>());
            }

            PageRequest request = new PageRequest(Request);
            Expression<Func<Function, bool>> funcExp = FilterHelper.GetExpression<Function>(request.FilterGroup);
            funcExp = funcExp.And(m => functionIds.Contains(m.Id));
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[] { new SortCondition("Area"), new SortCondition("Controller") };
            }

            var page = _securityManager.Functions.ToPage(funcExp,
                request.PageCondition,
                m => new { m.Id, m.Name, m.AccessType, m.Area, m.Controller });
            return Json(page.ToPageData());
        }
    }
}