// -----------------------------------------------------------------------
//  <copyright file="RoleEntityController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-05 14:45</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Liuliu.Demo.Identity.Entities;
using Liuliu.Demo.Security;
using Liuliu.Demo.Security.Dtos;
using Liuliu.Demo.Security.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Extensions;
using OSharp.Filter;
using OSharp.Secutiry;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 6, Position = "Security", PositionName = "权限安全模块")]
    [Description("管理-角色数据权限")]
    public class RoleEntityController : AdminApiController
    {
        private readonly SecurityManager _securityManager;
        private readonly IFilterService _filterService;

        public RoleEntityController(SecurityManager securityManager,
            IFilterService filterService)
        {
            _securityManager = securityManager;
            _filterService = filterService;
        }

        /// <summary>
        /// 读取角色数据权限列表信息
        /// </summary>
        /// <param name="request">页请求信息</param>
        /// <returns>角色数据权限列表分页信息</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("ReadProperties", Controller = "EntityInfo")]
        [Description("读取")]
        public PageData<EntityRoleOutputDto> Read(PageRequest request)
        {
            Expression<Func<EntityRole, bool>> predicate = _filterService.GetExpression<EntityRole>(request.FilterGroup);
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[]
                {
                    new SortCondition("RoleId"),
                    new SortCondition("EntityId"),
                    new SortCondition("Operation")
                };
            }
            RoleManager<Role> roleManager = HttpContext.RequestServices.GetService<RoleManager<Role>>();
            Func<EntityRole, bool> updateFunc = _filterService.GetDataFilterExpression<EntityRole>(null, DataAuthOperation.Update).Compile();
            Func<EntityRole, bool> deleteFunc = _filterService.GetDataFilterExpression<EntityRole>(null, DataAuthOperation.Delete).Compile();
            var page = _securityManager.EntityRoles.ToPage(predicate,
                request.PageCondition,
                m => new
                {
                    D = m,
                    RoleName = roleManager.Roles.First(n => n.Id == m.RoleId).Name,
                    EntityName = m.EntityInfo.Name,
                    EntityType = m.EntityInfo.TypeName,
                }).ToPageResult(data => data.Select(m => new EntityRoleOutputDto(m.D)
                {
                    RoleName = m.RoleName,
                    EntityName = m.EntityName,
                    EntityType = m.EntityType,
                    Updatable = updateFunc(m.D),
                    Deletable = deleteFunc(m.D)
                }).ToArray());
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
        [DependOnFunction("ReadNode", Controller = "Role")]
        [DependOnFunction("ReadNode", Controller = "EntityInfo")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("新增")]
        public async Task<AjaxResult> Create(params EntityRoleInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            
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
        [DependOnFunction("ReadNode", Controller = "Role")]
        [DependOnFunction("ReadNode", Controller = "EntityInfo")]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新")]
        public async Task<AjaxResult> Update(params EntityRoleInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
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
            Check.NotNull(ids, nameof(ids));
            
            OperationResult result = await _securityManager.DeleteEntityRoles(ids);
            return result.ToAjaxResult();
        }
    }
}