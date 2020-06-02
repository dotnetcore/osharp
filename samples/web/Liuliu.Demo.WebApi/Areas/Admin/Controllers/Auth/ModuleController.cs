// -----------------------------------------------------------------------
//  <copyright file="ModuleController.cs" company="OSharp开源团队">
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

using Liuliu.Demo.Authorization;
using Liuliu.Demo.Authorization.Dtos;
using Liuliu.Demo.Authorization.Entities;

using Microsoft.AspNetCore.Mvc;

using OSharp.Authorization.Functions;
using OSharp.Authorization.Modules;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Filter;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 1, Position = "Auth", PositionName = "权限授权模块")]
    [Description("管理-模块信息")]
    public class ModuleController : AdminApiController
    {
        private readonly FunctionAuthManager _functionAuthManager;
        private readonly IFilterService _filterService;

        public ModuleController(FunctionAuthManager functionAuthManager, IFilterService filterService)
        {
            _functionAuthManager = functionAuthManager;
            _filterService = filterService;
        }

        /// <summary>
        /// 读取模块信息
        /// </summary>
        /// <returns>模块信息集合</returns>
        [HttpPost]
        [ModuleInfo]
        [Description("读取")]
        public List<ModuleOutputDto> Read()
        {
            Expression<Func<Module, bool>> predicate = m => true;
            List<ModuleOutputDto> modules = _functionAuthManager.Modules.Where(predicate).OrderBy(m => m.OrderCode).ToOutput<Module, ModuleOutputDto>().ToList();
            return modules;
        }

        /// <summary>
        /// 读取模块[用户]树数据
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>模块[用户]树数据</returns>
        [HttpGet]
        [Description("读取模块[用户]树数据")]
        public List<object> ReadUserModules(int userId)
        {
            Check.GreaterThan(userId, nameof(userId), 0);
            int[] checkedModuleIds = _functionAuthManager.ModuleUsers.Where(m => m.UserId == userId).Select(m => m.ModuleId).ToArray();

            int[] rootIds = _functionAuthManager.Modules.Where(m => m.ParentId == null).OrderBy(m => m.OrderCode).Select(m => m.Id).ToArray();
            var result = GetModulesWithChecked(rootIds, checkedModuleIds);
            return result;
        }

        /// <summary>
        /// 读取模块[角色]树数据
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns>模块[角色]树数据</returns>
        [HttpGet]
        [Description("读取模块[角色]树数据")]
        public List<object> ReadRoleModules(int roleId)
        {
            Check.GreaterThan(roleId, nameof(roleId), 0);
            int[] checkedModuleIds = _functionAuthManager.ModuleRoles.Where(m => m.RoleId == roleId).Select(m => m.ModuleId).ToArray();

            int[] rootIds = _functionAuthManager.Modules.Where(m => m.ParentId == null).OrderBy(m => m.OrderCode).Select(m => m.Id).ToArray();
            var result = GetModulesWithChecked(rootIds, checkedModuleIds);
            return result;
        }
        
        private List<object> GetModulesWithChecked(int[] rootIds, int[] checkedModuleIds)
        {
            var modules = _functionAuthManager.Modules.Where(m => rootIds.Contains(m.Id)).OrderBy(m => m.OrderCode).Select(m => new
            {
                m.Id,
                m.Name,
                m.OrderCode,
                m.Remark,
                ChildIds = _functionAuthManager.Modules.Where(n => n.ParentId == m.Id).OrderBy(n => n.OrderCode).Select(n => n.Id).ToList()
            }).ToList();
            List<object> nodes = new List<object>();
            foreach (var item in modules)
            {
                if (item.ChildIds.Count == 0 && !IsRoleLimit(item.Id))
                {
                    continue;
                }
                var node = new
                {
                    item.Id,
                    item.Name,
                    item.OrderCode,
                    IsChecked = checkedModuleIds.Contains(item.Id),
                    HasChildren = item.ChildIds.Count > 0,
                    item.Remark,
                    Children = item.ChildIds.Count > 0 ? GetModulesWithChecked(item.ChildIds.ToArray(), checkedModuleIds) : new List<object>()
                };

                if (node.Children.Count == 0 && !IsRoleLimit(node.Id))
                {
                    continue;
                }

                nodes.Add(node);
            }
            return nodes;
        }

        private bool IsRoleLimit(int moduleId)
        {
            return _functionAuthManager.Functions
                .Where(m => _functionAuthManager.ModuleFunctions.Where(n => n.ModuleId == moduleId).Select(n => n.FunctionId).Contains(m.Id))
                .Any(m => m.AccessType == FunctionAccessType.RoleLimit);
        }

        /// <summary>
        /// 读取模块功能
        /// </summary>
        /// <returns>模块功能信息</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [Description("读取模块功能")]
        public PageData<FunctionOutputDto2> ReadFunctions(PageRequest request)
        {
            if (request.FilterGroup.Rules.Count == 0)
            {
                return new PageData<FunctionOutputDto2>();
            }
            Expression<Func<Module, bool>> moduleExp = _filterService.GetExpression<Module>(request.FilterGroup);
            int[] moduleIds = _functionAuthManager.Modules.Where(moduleExp).Select(m => m.Id).ToArray();
            Guid[] functionIds = _functionAuthManager.ModuleFunctions.Where(m => moduleIds.Contains(m.ModuleId))
                .Select(m => m.FunctionId).Distinct().ToArray();
            if (functionIds.Length == 0)
            {
                return new PageData<FunctionOutputDto2>();
            }
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[] { new SortCondition("Area"), new SortCondition("Controller") };
            }
            var page = _functionAuthManager.Functions.ToPage(m => functionIds.Contains(m.Id),
                request.PageCondition,
                m => new FunctionOutputDto2() { Id = m.Id, Name = m.Name, AccessType = m.AccessType, Area = m.Area, Controller = m.Controller });
            return page.ToPageData();
        }

        //模块暂时不需要CUD操作
        /*
        /// <summary>
        /// 新增模块子节点
        /// </summary>
        /// <param name="dto">模块信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [UnitOfWork]
        [Description("新增子节点")]
        public async Task<AjaxResult> Create(ModuleInputDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            OperationResult result = await _functionAuthManager.CreateModule(dto);
            return result.ToAjaxResult();
        }

        /// <summary>
        /// 更新模块信息
        /// </summary>
        /// <param name="dto">模块信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [UnitOfWork]
        [Description("更新")]
        public async Task<AjaxResult> Update(ModuleInputDto dto)
        {
            Check.NotNull(dto, nameof(dto));
            if (dto.Id == 1)
            {
                return new AjaxResult("根节点不能编辑", AjaxResultType.Error);
            }

            OperationResult result = await _functionAuthManager.UpdateModule(dto);
            return result.ToAjaxResult();
        }

        /// <summary>
        /// 删除模块信息
        /// </summary>
        /// <param name="id">模块信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [UnitOfWork]
        [Description("删除")]
        public async Task<AjaxResult> Delete([FromForm] int id)
        {
            Check.NotNull(id, nameof(id));
            Check.GreaterThan(id, nameof(id), 0);
            if (id == 1)
            {
                return new AjaxResult("根节点不能删除", AjaxResultType.Error);
            }

            OperationResult result = await _functionAuthManager.DeleteModule(id);
            return result.ToAjaxResult();
        }

        /// <summary>
        /// 模块设置功能信息
        /// </summary>
        /// <param name="dto">设置信息</param>
        /// <returns>JSON操作结果</returns>
        [HttpPost]
        [ModuleInfo]
        [DependOnFunction("Read")]
        [DependOnFunction("ReadTreeNode", Controller = "Function")]
        [UnitOfWork]
        [Description("设置功能")]
        public async Task<AjaxResult> SetFunctions([FromBody] ModuleSetFunctionDto dto)
        {
            OperationResult result = await _functionAuthManager.SetModuleFunctions(dto.ModuleId, dto.FunctionIds);
            return result.ToAjaxResult();
        }
        */
    }
}