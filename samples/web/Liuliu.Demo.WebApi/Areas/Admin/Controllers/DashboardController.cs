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

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore.Mvc;
using OSharp.Authorization;
using OSharp.Authorization.Functions;
using OSharp.Authorization.Modules;
using OSharp.Entity;
using OSharp.Hosting.Apis.Areas.Admin.Controllers;
using OSharp.Hosting.Authorization;
using OSharp.Hosting.Identity.Entities;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 1)]
    [Description("管理-信息汇总")]
    public class DashboardController : AdminApiControllerBase
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// 初始化一个<see cref="DashboardController"/>类型的新实例
        /// </summary>
        public DashboardController(IServiceProvider provider) : base(provider)
        {
            _provider = provider;
        }

        private UserManager<User> UserManager => _provider.GetRequiredService<UserManager<User>>();

        private RoleManager<Role> RoleManager => _provider.GetRequiredService<RoleManager<Role>>();

        private FunctionAuthManager FunctionAuthManager => _provider.GetRequiredService<FunctionAuthManager>();

        private DataAuthManager DataAuthManager => _provider.GetRequiredService<DataAuthManager>();

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns>统计数据</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("统计数据")]
        public object SummaryData(DateTime start, DateTime end)
        {
            IFunction function = this.GetExecuteFunction();
            Expression<Func<User, bool>> userExp = GetExpression<User>(start, end);

            var users = CacheService.ToCacheList(UserManager.Users.Where(userExp).GroupBy(m => 1).Select(g => new
            {
                TotalCount = g.Sum(n => 1),
                ValidCount = g.Sum(n => n.EmailConfirmed ? 1 : 0)
            }),
                function,
                "Dashboard_Summary_User",
                start,
                end).FirstOrDefault() ?? new { TotalCount = 0, ValidCount = 0 };
            var roles = CacheService.ToCacheList(RoleManager.Roles.GroupBy(m => 1).Select(g => new
            {
                TotalCount = g.Sum(n => 1),
                AdminCount = g.Sum(n => n.IsAdmin ? 1 : 0)
            }),
                function,
                "Dashboard_Summary_Role",
                start,
                end).FirstOrDefault() ?? new { TotalCount = 0, AdminCount = 0 };
            var modules = CacheService.ToCacheList(FunctionAuthManager.Modules.GroupBy(m => 1).Select(g => new
            {
                TotalCount = g.Sum(n => 1),
                SiteCount = g.Sum(n => n.TreePathString.Contains("$2$") ? 1 : 0),
                AdminCount = g.Sum(m => m.TreePathString.Contains("$3$") ? 1 : 0)
            }),
                function,
                "Dashboard_Summary_Module").FirstOrDefault() ?? new { TotalCount = 0, SiteCount = 0, AdminCount = 0 };
            var functions = CacheService.ToCacheList(FunctionAuthManager.Functions.GroupBy(m => m.Id).Select(g => new
            {
                TotalCount = g.Sum(n => 1),
                ControllerCount = g.Sum(m => m.IsController ? 1 : 0)
            }),
                function,
                "Dashboard_Summary_Function").FirstOrDefault() ?? new { TotalCount = 0, ControllerCount = 0 };
            var entityInfos = CacheService.ToCacheList(DataAuthManager.EntityInfos.GroupBy(m => m.Id).Select(g => new
            {
                TotalCount = g.Sum(n => 1),
                AuditCount = g.Sum(m => m.AuditEnabled ? 1 : 0)
            }),
                function,
                "Dashboard_Summary_EntityInfo").FirstOrDefault() ?? new { TotalCount = 0, AuditCount = 0 };

            var data = new { users, roles, modules, functions, entityInfos };
            return data;
        }

        [HttpGet]
        [ModuleInfo]
        [Description("曲线数据")]
        public object LineData(DateTime start, DateTime end)
        {
            IFunction function = this.GetExecuteFunction();
            Expression<Func<User, bool>> userExp = GetExpression<User>(start, end);

            var userData = CacheService.ToCacheList(UserManager.Users.Where(userExp).GroupBy(m => m.CreatedTime.Date).Select(g => new
            {
                Date = g.Key,
                DailyCount = g.Count()
            }),
                function,
                "Dashboard_Line_User",
                start,
                end);
            var users = userData.Select(m => new
            {
                Date = m.Date.ToString("d"),
                m.DailyCount,
                DailySum = userData.Where(n => n.Date <= m.Date).Sum(n => n.DailyCount)
            }).ToList();

            return users;
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
