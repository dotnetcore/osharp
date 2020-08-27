// -----------------------------------------------------------------------
//  <copyright file="AuditOperationController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 14:47</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq.Expressions;

using Microsoft.AspNetCore.Mvc;

using OSharp.Authorization.Modules;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Hosting.Systems;
using OSharp.Hosting.Systems.Dtos;
using OSharp.Hosting.Systems.Entities;


namespace OSharp.Hosting.Apis.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 2, Position = "Systems", PositionName = "系统管理模块")]
    [Description("管理-操作审计信息")]
    public class AuditOperationController : AdminApiControllerBase
    {
        private readonly IAuditContract _auditContract;
        private readonly IFilterService _filterService;

        public AuditOperationController(IAuditContract auditContract, IFilterService filterService)
        {
            _auditContract = auditContract;
            _filterService = filterService;
        }

        /// <summary>
        /// 读取操作审计信息
        /// </summary>
        /// <param name="request">页数据请求</param>
        /// <returns>操作审计信息的页数据</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public PageData<AuditOperationOutputDto> Read(PageRequest request)
        {
            Expression<Func<AuditOperation, bool>> predicate = _filterService.GetExpression<AuditOperation>(request.FilterGroup);
            request.AddDefaultSortCondition(new SortCondition("CreatedTime", ListSortDirection.Descending));
            var page = _auditContract.AuditOperations.ToPage<AuditOperation, AuditOperationOutputDto>(predicate, request.PageCondition);
            return page.ToPageData();
        }
    }
}
