// -----------------------------------------------------------------------
//  <copyright file="FunctionController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-04 15:40</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.UI;
using OSharp.Data;
using OSharp.Demo.Security;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Core;
using OSharp.Core.Functions;
using OSharp.Security;


namespace OSharp.Demo.Web.Areas.Admin.Controllers
{
    [Description("管理-功能信息")]
    [Area("Admin")]
    [Route("api/[area]/[controller]/[action]")]
    public class FunctionController : Controller
    {
        private readonly SecurityManager _securityManager;

        public FunctionController(SecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        [Description("读取")]
        public IActionResult Read()
        {
            PageRequest request = new PageRequest(Request);
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[]
                {
                    new SortCondition("Area"),
                    new SortCondition("Controller"),
                    new SortCondition("Action")
                };
            }
            Expression<Func<Function, bool>> predicate = FilterHelper.GetExpression<Function>(request.FilterGroup);
            var page = _securityManager.Functions.ToPage(predicate, request.PageCondition, m => new
            {
                Id = m.Id.ToString("N"),
                m.Name,
                m.Area,
                m.Controller,
                m.Action,
                m.IsController,
                m.IsAjax,
                m.AccessType,
                m.IsAccessTypeChanged,
                m.AuditOperationEnabled,
                m.AuditEntityEnabled,
                m.CacheExpirationSeconds,
                m.IsCacheSliding,
                m.IsLocked
            });
            return Json(page.ToPageData());
        }

        [HttpPost]
        [Description("更新")]
        public async Task<IActionResult> Update(FunctionInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));

            OperationResult result = await _securityManager.UpdateFunctions(dtos);
            return Json(result.ToAjaxResult());
        }
    }
}