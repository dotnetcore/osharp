// -----------------------------------------------------------------------
//  <copyright file="MenuInfoController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-01 10:22</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Authorization.Modules;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Hosting.Systems;
using OSharp.Hosting.Systems.Dtos;
using OSharp.Hosting.Systems.Entities;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 5, Position = "Systems", PositionName = "系统管理模块")]
    [Description("管理-菜单信息")]
    public class MenuController : AdminApiControllerBase
    {
        private readonly ISystemsContract _systemsContract;
        private readonly IFilterService _filterService;

        public MenuController(ISystemsContract systemsContract, IFilterService filterService)
        {
            _systemsContract = systemsContract;
            _filterService = filterService;
        }

        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public AjaxResult Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));

            Expression<Func<Menu, bool>> predicate = _filterService.GetExpression<Menu>(request.FilterGroup);
            var page = _systemsContract.MenuInfos.ToPage<Menu, MenuOutputDto>(predicate, request.PageCondition);

            return new AjaxResult("数据读取成功", AjaxResultType.Success, page.ToPageData());
        }

        [HttpPost]
        [ModuleInfo]
        [DependOnFunction(nameof(Read), Controller = nameof(MenuController))]
        [UnitOfWork]
        [Description("新增")]
        public async Task<AjaxResult> Create(MenuInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));

            OperationResult result = await _systemsContract.CreateMenuInfos(dtos);
            return result.ToAjaxResult();
        }

        [HttpPost]
        [ModuleInfo]
        [DependOnFunction(nameof(Read))]
        [UnitOfWork]
        [Description("更新")]
        public async Task<AjaxResult> Update(MenuInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));

            OperationResult result = await _systemsContract.UpdateMenuInfos(dtos);
            return result.ToAjaxResult();
        }

        [HttpPost]
        [ModuleInfo]
        [DependOnFunction(nameof(Read))]
        [UnitOfWork]
        [Description("删除")]
        public async Task<AjaxResult> Delete(int[] ids)
        {
            Check.NotNull(ids, nameof(ids));

            OperationResult result = await _systemsContract.DeleteMenuInfos(ids);
            return result.ToAjaxResult();
        }
    }
}