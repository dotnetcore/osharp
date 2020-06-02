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

using Liuliu.Demo.Systems.Dtos;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore.Mvc;
using OSharp.Authorization.Functions;
using OSharp.Authorization.Modules;
using OSharp.Caching;
using OSharp.Core.Packs;
using OSharp.Filter;
using OSharp.Reflection;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 4, Position = "Systems", PositionName = "系统管理模块")]
    [Description("管理-模块包信息")]
    public class PackController : AdminApiController
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
        public PageData<PackOutputDto> Read(PageRequest request)
        {
            request.AddDefaultSortCondition(
                new SortCondition("Level"),
                new SortCondition("Order")
            );
            IFunction function = this.GetExecuteFunction();
            Expression<Func<OsharpPack, bool>> exp = _filterService.GetExpression<OsharpPack>(request.FilterGroup);
            IServiceProvider provider = HttpContext.RequestServices;

            return _cacheService.ToPageCache(provider.GetAllPacks().AsQueryable(), exp,
                request.PageCondition,
                m => new PackOutputDto()
                {
                    Name = m.GetType().Name,
                    Display = m.GetType().GetDescription(true),
                    Class = m.GetType().FullName,
                    Level = m.Level,
                    Order = m.Order,
                    IsEnabled = m.IsEnabled
                },
                function).ToPageData();
        }
    }
}