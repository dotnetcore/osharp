// -----------------------------------------------------------------------
//  <copyright file="FunctionController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:49</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Liuliu.Demo.Common.Dtos;
using Liuliu.Demo.Security;
using Liuliu.Demo.Security.Dtos;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Security;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 2, Position = "Security")]
    [Description("管理-功能信息")]
    public class FunctionController : AdminApiController
    {
        private readonly SecurityManager _securityManager;

        public FunctionController(SecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        /// <summary>
        /// 读取功能信息
        /// </summary>
        /// <returns>功能信息集合</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public PageData<FunctionOutputDto> Read()
        {
            PageRequest request = new PageRequest(Request);
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[]
                {
                    new SortCondition("Area"),
                    new SortCondition("Controller"),
                    new SortCondition("IsController", ListSortDirection.Descending)
                };
            }
            Expression<Func<Function, bool>> predicate = FilterHelper.GetExpression<Function>(request.FilterGroup);
            PageResult<FunctionOutputDto> page = _securityManager.Functions.ToPage<Function, FunctionOutputDto>(predicate, request.PageCondition);
            return page.ToPageData();
        }

        /// <summary>
        /// 读取功能[模块]树数据
        /// </summary>
        /// <param name="moduleId">模块编号</param>
        /// <returns>功能[模块]树数据</returns>
        [HttpGet]
        [Description("读取功能[模块]树数据")]
        public TreeNode[] ReadTreeNode(int moduleId)
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
            return new[] { root };
        }

        /// <summary>
        /// 更新功能信息
        /// </summary>
        /// <param name="dtos">功能信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新")]
        public async Task<AjaxResult> Update(FunctionInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));

            OperationResult result = await _securityManager.UpdateFunctions(dtos);
            return result.ToAjaxResult();
        }
    }
}