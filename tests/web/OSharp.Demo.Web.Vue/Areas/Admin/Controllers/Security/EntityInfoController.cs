// -----------------------------------------------------------------------
//  <copyright file="EntityInfoController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-18 16:06</last-date>
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
using OSharp.Security;


namespace OSharp.Demo.Web_Vue.Areas.Admin.Controllers
{
    [Description("管理-实体信息")]
    [Area("Admin")]
    [Route("api/[area]/[controller]/[action]")]
    public class EntityInfoController : Controller
    {
        private readonly SecurityManager _securityManager;

        public EntityInfoController(SecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        [Description("读取")]
        public IActionResult Read()
        {
            PageRequest request = new PageRequest(Request);
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[] { new SortCondition("TypeName") };
            }
            Expression<Func<EntityInfo, bool>> predicate = FilterHelper.GetExpression<EntityInfo>(request.FilterGroup);
            var page = _securityManager.EntityInfos.ToPage(predicate,
                request.PageCondition,
                m => new
                {
                    Id = m.Id.ToString("N"),
                    m.Name,
                    m.TypeName,
                    m.AuditEnabled
                });
            return Json(page.ToPageData());
        }

        [HttpPost]
        [Description("更新")]
        public async Task<IActionResult> Update(EntityInfoInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            OperationResult result = await _securityManager.UpdateEntityInfos(dtos);
            return Json(result.ToAjaxResult());
        }
    }
}