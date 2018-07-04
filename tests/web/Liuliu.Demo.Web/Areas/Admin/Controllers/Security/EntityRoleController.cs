// -----------------------------------------------------------------------
//  <copyright file="EntityRoleController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-04 14:48</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Liuliu.Demo.Security;
using Liuliu.Demo.Security.Dtos;
using Liuliu.Demo.Security.Entities;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Filter;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 6, Position = "Security")]
    [Description("管理-角色数据权限")]
    public class EntityRoleController : AdminApiController
    {
        private readonly SecurityManager _securityManager;

        public EntityRoleController(SecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        /// <summary>
        /// 读取角色数据权限列表信息
        /// </summary>
        /// <returns>角色数据权限列表信息</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public PageData<EntityRoleOutputDto> Read()
        {
            PageRequest request = new PageRequest(Request);
            Expression<Func<EntityRole, bool>> predicate = FilterHelper.GetExpression<EntityRole>(request.FilterGroup);
            var page = _securityManager.EntityRoles.ToPage<EntityRole, EntityRoleOutputDto>(predicate, request.PageCondition);
            return page.ToPageData();
        }

        /// <summary>
        /// 新增角色数据权限信息
        /// </summary>
        /// <param name="dtos">角色数据权限信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("新增")]
        public async Task<AjaxResult> Create(params EntityRoleInputDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = await _securityManager.CreateEntityRoles(dtos);
            return result.ToAjaxResult();
        }

        /// <summary>
        /// 更新角色数据权限信息
        /// </summary>
        /// <param name="dtos">角色数据权限信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新")]
        public async Task<AjaxResult> Update(params EntityRoleInputDto[] dtos)
        {
            dtos.CheckNotNull("dtos");
            OperationResult result = await _securityManager.UpdateEntityRoles(dtos);
            return result.ToAjaxResult();
        }

        /// <summary>
        /// 删除角色数据权限信息
        /// </summary>
        /// <param name="ids">角色数据权限信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除")]
        public async Task<AjaxResult> Delete(params Guid[] ids)
        {
            ids.CheckNotNull("ids");
            OperationResult result = await _securityManager.DeleteEntityRoles(ids);
            return result.ToAjaxResult();
        }
    }
}