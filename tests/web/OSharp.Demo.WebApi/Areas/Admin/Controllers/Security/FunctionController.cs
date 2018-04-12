// -----------------------------------------------------------------------
//  <copyright file="FunctionController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-04-12 21:31</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.UI;
using OSharp.Core.Functions;
using OSharp.Data;
using OSharp.Demo.Security;
using OSharp.Demo.WebApi.Areas.Admin.ViewModels;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Security;


namespace OSharp.Demo.WebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Description("管理-功能信息")]
    public class FunctionController : AreaApiController
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
            var page = _securityManager.Functions.ToPage(predicate,
                request.PageCondition,
                m => new
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

        [Description("读取功能[模块]树数据")]
        public IActionResult ReadTreeNode(int moduleId)
        {
            Check.GreaterThan(moduleId, nameof(moduleId), 0);
            Guid[] checkFuncIds = _securityManager.ModuleFunctions.Where(m => m.ModuleId == moduleId).Select(m => m.FunctionId).ToArray();

            var groups = _securityManager.Functions.Unlocked()
                .Where(m => m.Area == null || m.Area == "Admin")
                .Select(m => new
                {
                    m.Id,
                    m.Name,
                    m.Area,
                    m.Controller,
                    m.Action,
                    m.IsController,
                    m.AccessType
                }).ToList().GroupBy(m => m.Area).OrderBy(m => m.Key).ToList();

            TreeNode root = new TreeNode { Id = Guid.NewGuid().ToString("N"), Name = "系统", HasChildren = true };
            foreach (var group1 in groups)
            {
                TreeNode areaNode = new TreeNode { Id = null, Name = group1.Key == null ? "网站" : group1.Key == "Admin" ? "管理" : "未知模块" };
                root.Items.Add(areaNode);
                var group2S = group1.GroupBy(m => m.Controller).OrderBy(m => m.Key).ToList();
                foreach (var group2 in group2S)
                {
                    areaNode.HasChildren = true;
                    var controller = group2.First(m => m.Action == null);
                    TreeNode controllerNode =
                        new TreeNode() { Id = Guid.NewGuid().ToString("N"), Name = $"{controller.Name}[{controller.Controller}]" };
                    areaNode.Items.Add(controllerNode);
                    foreach (var action in group2.Where(m => m.Action != null).OrderBy(m => m.Action))
                    {
                        controllerNode.HasChildren = true;
                        TreeNode actionNode = new TreeNode()
                        {
                            Id = action.Id.ToString("N"),
                            Name = action.Area == null
                                ? $"{action.Name}[{action.Controller}/{action.Action}]"
                                : $"{action.Name}[{action.Area}/{action.Controller}/{action.Action}]",
                            Data = action.AccessType,
                            IsChecked = checkFuncIds.Contains(action.Id)
                        };
                        controllerNode.Items.Add(actionNode);
                    }
                }
            }
            return Json(new[] { root });
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