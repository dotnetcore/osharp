using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.UI;
using OSharp.Core.Functions;
using OSharp.Data;
using OSharp.Demo.Security;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Security;


namespace OSharp.Demo.WebApi.Areas.Admin.Controllers
{
    [Description("管理-功能信息")]
    public class FunctionController : AdminController
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
