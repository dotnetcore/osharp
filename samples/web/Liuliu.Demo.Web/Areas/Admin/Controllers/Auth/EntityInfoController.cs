// -----------------------------------------------------------------------
//  <copyright file="EntityInfoController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:49</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Liuliu.Demo.Authorization;
using Liuliu.Demo.Authorization.Dtos;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Authorization.Dtos;
using OSharp.Authorization.EntityInfos;
using OSharp.Authorization.Modules;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Filter;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 5, Position = "Auth", PositionName = "权限授权模块")]
    [Description("管理-实体信息")]
    public class EntityInfoController : AdminApiController
    {
        private readonly DataAuthManager _dataAuthManager;
        private readonly IFilterService _filterService;

        public EntityInfoController(DataAuthManager dataAuthManager,
            IFilterService filterService)
        {
            _dataAuthManager = dataAuthManager;
            _filterService = filterService;
        }

        /// <summary>
        /// 读取实体信息
        /// </summary>
        /// <returns>实体信息集合</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public PageData<EntityInfoOutputDto> Read(PageRequest request)
        {
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[] { new SortCondition("TypeName") };
            }
            Expression<Func<EntityInfo, bool>> predicate = _filterService.GetExpression<EntityInfo>(request.FilterGroup);
            var page = _dataAuthManager.EntityInfos.ToPage<EntityInfo, EntityInfoOutputDto>(predicate, request.PageCondition);
            return page.ToPageData();
        }

        /// <summary>
        /// 读取实体节点
        /// </summary>
        /// <returns>实体节点集合</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("读取节点")]
        public List<EntityInfoNode> ReadNode()
        {
            List<EntityInfoNode> nodes = _dataAuthManager.EntityInfos.OrderBy(m => m.TypeName).ToOutput<EntityInfo, EntityInfoNode>().ToList();
            return nodes;
        }

        /// <summary>
        /// 读取实体属性信息
        /// </summary>
        /// <param name="typeName">实体类型</param>
        /// <returns>JSON操作结果</returns>
        [HttpGet]
        [Description("读取实体属性信息")]
        public AjaxResult ReadProperties(string typeName)
        {
            Check.NotNull(typeName, nameof(typeName));
            string json = _dataAuthManager.EntityInfos.FirstOrDefault(m => m.TypeName == typeName)?.PropertyJson;
            if (json == null)
            {
                return new AjaxResult($"实体类“{typeName}”不存在", AjaxResultType.Error);
            }
            string[] filterTokens = { "Normalized", "Stamp", "Password" };
            EntityProperty[] properties = json.FromJsonString<EntityProperty[]>().Where(m => !filterTokens.Any(n => m.Name.Contains(n)))
                .OrderByDescending(m => m.Name == "Id").ToArray();
            return new AjaxResult("获取成功", AjaxResultType.Success, properties);
        }

        /// <summary>
        /// 更新实体信息
        /// </summary>
        /// <param name="dtos">实体信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [UnitOfWork]
        [Description("更新")]
        public async Task<AjaxResult> Update(EntityInfoInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            OperationResult result = await _dataAuthManager.UpdateEntityInfos(dtos);
            return result.ToAjaxResult();
        }
    }
}