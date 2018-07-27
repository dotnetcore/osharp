// -----------------------------------------------------------------------
//  <copyright file="DashboardController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2018-07-26 16:07</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

using Liuliu.Demo.Identity;
using Liuliu.Demo.Identity.Entities;
using Liuliu.Demo.Security;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using OSharp.AspNetCore.Mvc;
using OSharp.Caching;
using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Entity;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [Description("管理-信息汇总")]
    public class DashboardController : AdminApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SecurityManager _securityManager;

        /// <summary>
        /// 初始化一个<see cref="DashboardController"/>类型的新实例
        /// </summary>
        public DashboardController(UserManager<User> userManager, RoleManager<Role> roleManager, SecurityManager securityManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _securityManager = securityManager;
        }

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns>统计数据</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("统计")]
        [AllowAnonymous]
        public IActionResult SummaryData(DateTime? start, DateTime? end)
        {
            IFunction function = this.GetExecuteFunction();
            Expression<Func<User, bool>> userExp = m => true;//GetExpression<User>(start, end);
            //Expression<Func<Role, bool>> roleExp = GetExpression<Role>(start, end);

            var user = _userManager.Users.Where(userExp).GroupBy(m => 1).Select(g => new
            {
                TotalCount = g.Sum(m => 1),
                ValidCount = g.Count(n => n.EmailConfirmed)
            }).ToCacheList(function, "Dashboard_Summary_User", start, end).FirstOrDefault();












            return Json(user);
        }

        private static Expression<Func<TEntity, bool>> GetExpression<TEntity>(DateTime start, DateTime end)
            where TEntity : class, ICreatedTime
        {
            if (start > end)
            {
                throw new ArgumentException($"结束时间{end}不能小于开始时间{start}");
            }
            return m => m.CreatedTime.Date >= start.Date && m.CreatedTime.Date <= end.Date;
        }
    }
}