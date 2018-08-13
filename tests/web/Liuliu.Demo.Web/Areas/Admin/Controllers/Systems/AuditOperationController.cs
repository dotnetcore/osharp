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

using Liuliu.Demo.Systems;
using Liuliu.Demo.Systems.Dtos;
using Liuliu.Demo.Systems.Entities;

using Microsoft.AspNetCore.Mvc;

using OSharp.Core.Modules;
using OSharp.Entity;
using OSharp.Filter;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 2, Position = "Systems", PositionName = "系统管理模块")]
    [Description("管理-操作审计信息")]
    public class AuditOperationController : AdminApiController
    {
        private readonly IAuditContract _auditContract;

        public AuditOperationController(IAuditContract auditContract)
        {
            _auditContract = auditContract;
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
            Expression<Func<AuditOperation, bool>> predicate = FilterHelper.GetExpression<AuditOperation>(request.FilterGroup);
            request.AddDefaultSortCondition(new SortCondition("CreatedTime", ListSortDirection.Descending));
            var page = _auditContract.AuditOperations.ToPage<AuditOperation, AuditOperationOutputDto>(predicate, request.PageCondition);
            return page.ToPageData();
        }
    }
}