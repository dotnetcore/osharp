// -----------------------------------------------------------------------
//  <copyright file="SettingsController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OSharp.Hosting.MultiTenancy;
using OSharp.Hosting.MultiTenancy.Dtos;
using OSharp.Hosting.MultiTenancy.Entities;
using OSharp.Hosting.Systems.Dtos;
using Liuliu.Demo.Web.Startups;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Authorization.Modules;
using OSharp.Core.Systems;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Entity.KeyGenerate;
using OSharp.Hosting.Apis.Areas.Admin.Controllers;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 1, Position = "MultiTenancy", PositionName = "多租户模块")]
    [Description("管理-租户管理")]
    public class TenantController : AdminApiControllerBase
    {

        private readonly IMultiTenancyContract MultiTenancyContract;
        private readonly IKeyGenerator<long> IDMaker;
        private readonly TenantDatabaseMigrator TenantDatabaseMigrator;
        

        /// <summary>
        /// 初始化一个<see cref="TenantController"/>类型的新实例
        /// </summary>
        public TenantController(IServiceProvider provider)
            : base(provider)
        {
            MultiTenancyContract = provider.GetService<IMultiTenancyContract>();
            IDMaker = provider.GetService<IKeyGenerator<long>>();
            TenantDatabaseMigrator = provider.GetService<TenantDatabaseMigrator>();
        }

        /// <summary>
        /// 读取租户列表信息
        /// </summary>
        /// <param name="request">页请求信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("列表")]
        public AjaxResult<PageData<TenantListOutputDto>> Read(PageRequest request)
        {
            Check.NotNull(request, nameof(request));

            if (request.PageCondition.PageSize > 100)
                return new AjaxResult<PageData<TenantListOutputDto>>("请求参数异常", AjaxResultType.Error);

            Expression<Func<Tenant, bool>> predicate = FilterService.GetExpression<Tenant>(request.FilterGroup);
            var page = MultiTenancyContract.Tenants.ToPage(predicate,
                request.PageCondition,
                m => new
                {
                    Entity = m
                }).ToPageResult(data => data.Select(m => new TenantListOutputDto(m.Entity)
                {

                }).ToArray());
            return new AjaxResult<PageData<TenantListOutputDto>>(page.ToPageData());
        }

        /// <summary>
        /// 读取租户详情
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <returns>JSON操作结果</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("详情")]
        public AjaxResult<TenantOutputDto> ReadOne(long id)
        {
            Check.NotNull(id, nameof(id));
            var entity = MultiTenancyContract.Tenants.FirstOrDefault(p => p.Id == id);
            if (entity == null)
                return new AjaxResult<TenantOutputDto>( "获取失败,未找到对应数据", AjaxResultType.Error);
            else
            {
                var outputDto = new TenantOutputDto(entity)
                {
                };
                return new AjaxResult<TenantOutputDto>("获取成功", outputDto);
            }
        }


        /// <summary>
        /// 新增租户信息
        /// </summary>
        /// <param name="dtos">租户信息输入DTO</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [UnitOfWork]
        [Description("新增")]
        public async Task<AjaxResult> Create(TenantInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));

            var ids = new List<long>();
            foreach (var dto in dtos)
            {
                dto.Id = IDMaker.Create();
                ids.Add(dto.Id);
            }
            OperationResult result = await MultiTenancyContract.CreateTenants(dtos);
            result.Data = ids;
            return result.ToAjaxResult(true);
        }

        /// <summary>
        /// 更新租户信息
        /// </summary>
        /// <param name="dtos">租户信息输入DTO</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [UnitOfWork]
        [Description("更新")]
        public async Task<AjaxResult> Update(TenantInputDto[] dtos)
        {
            Check.NotNull(dtos, nameof(dtos));
            OperationResult result = await MultiTenancyContract.UpdateTenants(dtos);
            return result.ToAjaxResult();
        }

        /// <summary>
        /// 启用租户
        /// </summary>
        /// <param name="id">租户信息编号</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [UnitOfWork]
        [Description("启用租户")]
        public async Task<AjaxResult> Enable(long id)
        {
            Check.NotNull(id, nameof(id));
            OperationResult result = await MultiTenancyContract.SetTenantEnable(id);
            return result.ToAjaxResult();
        }

        /// <summary>
        /// 禁用租户
        /// </summary>
        /// <param name="id">租户信息编号</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [UnitOfWork]
        [Description("禁用租户")]
        public async Task<AjaxResult> Disable(long id)
        {
            Check.NotNull(id, nameof(id));
            OperationResult result = await MultiTenancyContract.SetTenantDisable(id);
            return result.ToAjaxResult();
        }

        /// <summary>
        /// 删除租户信息
        /// </summary>
        /// <param name="ids">租户信息编号</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [UnitOfWork]
        [Description("删除")]
        public async Task<AjaxResult> Delete(long[] ids)
        {
            Check.NotNull(ids, nameof(ids));
            OperationResult result = await MultiTenancyContract.DeleteTenants(ids);
            return result.ToAjaxResult();
        }

        /// <summary>
        /// 迁移指定租户
        /// </summary>
        /// <param name="id">租户信息编号</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [UnitOfWork]
        [Description("迁移指定租户")]
        public AjaxResult Migrator(long id)
        {
            Check.NotNull(id, nameof(id));
            var entity = MultiTenancyContract.Tenants.FirstOrDefault(p => p.Id == id);
            if (entity == null)
                return new AjaxResult<TenantOutputDto>("获取失败,未找到指定租户", AjaxResultType.Error);
            else
            {
                var outputDto = new TenantOutputDto(entity)
                {
                };
                var result = TenantDatabaseMigrator.MigrateTenant(outputDto);
                return result.ToAjaxResult();
            }
        }

        /// <summary>
        /// 迁移所有租户
        /// </summary>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [UnitOfWork]
        [Description("迁移所有租户")]
        public AjaxResult MigratorAll()
        {
            var result = TenantDatabaseMigrator.MigrateAllTenants();
            return result.ToAjaxResult(true);
        }
    }
}
