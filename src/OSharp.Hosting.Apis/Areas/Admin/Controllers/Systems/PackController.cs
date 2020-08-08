// -----------------------------------------------------------------------
//  <copyright file="PackController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-13 15:00</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.UI;
using OSharp.Authorization.Functions;
using OSharp.Authorization.Modules;
using OSharp.Caching;
using OSharp.Core.Packs;
using OSharp.Data;
using OSharp.Filter;
using OSharp.Hosting.Systems.Dtos;
using OSharp.Reflection;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 4, Position = "Systems", PositionName = "系统管理模块")]
    [Description("管理-模块包信息")]
    public class PackController : AdminApiControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly IFilterService _filterService;

        /// <summary>
        /// 初始化一个<see cref="PackController"/>类型的新实例
        /// </summary>
        public PackController(ICacheService cacheService,
            IFilterService filterService)
        {
            _cacheService = cacheService;
            _filterService = filterService;
        }

        /// <summary>
        /// 读取模块包列表信息
        /// </summary>
        /// <param name="request">页请求</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public AjaxResult Read(PageRequest request)
        {
            request.AddDefaultSortCondition(
                new SortCondition("Level"),
                new SortCondition("Order")
            );
            IFunction function = this.GetExecuteFunction();
            Expression<Func<PackOutputDto, bool>> exp = _filterService.GetExpression<PackOutputDto>(request.FilterGroup);
            IServiceProvider provider = HttpContext.RequestServices;
            IQueryable<PackOutputDto> query = provider.GetAllPacks().Select(m => new PackOutputDto()
            {
                Name = m.GetType().Name,
                Display = m.GetType().GetDescription(true),
                Class = m.GetType().FullName,
                Level = m.Level,
                Order = m.Order,
                IsEnabled = m.IsEnabled
            }).AsQueryable();

            var page = _cacheService.ToPageCache(query,
                exp,
                request.PageCondition,
                m => m,
                function);
            return new AjaxResult("数据读取成功", AjaxResultType.Success, page.ToPageData());
        }
    }
}
