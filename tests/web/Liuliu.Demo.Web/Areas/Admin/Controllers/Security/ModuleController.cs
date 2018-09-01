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

using Liuliu.Demo.Security;
using Liuliu.Demo.Security.Dtos;
using Liuliu.Demo.Security.Entities;

using Microsoft.AspNetCore.Mvc;

using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Filter;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 1, Position = "Security", PositionName = "权限安全模块")]
    [Description("管理-模块信息")]
    public class ModuleController : AdminApiController
    {
        private readonly SecurityManager _securityManager;

        public ModuleController(SecurityManager securityManager)
        {
            _securityManager = securityManager;
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
            List<ModuleOutputDto> modules = _securityManager.Modules.Where(predicate).OrderBy(m => m.OrderCode).ToOutput<Module, ModuleOutputDto>().ToList();
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
            int[] checkedModuleIds = _securityManager.ModuleUsers.Where(m => m.UserId == userId).Select(m => m.ModuleId).ToArray();

            int[] rootIds = _securityManager.Modules.Where(m => m.ParentId == null).OrderBy(m => m.OrderCode).Select(m => m.Id).ToArray();
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
            int[] checkedModuleIds = _securityManager.ModuleRoles.Where(m => m.RoleId == roleId).Select(m => m.ModuleId).ToArray();

            int[] rootIds = _securityManager.Modules.Where(m => m.ParentId == null).OrderBy(m => m.OrderCode).Select(m => m.Id).ToArray();
            var result = GetModulesWithChecked(rootIds, checkedModuleIds);
            return result;
        }
        
        private List<object> GetModulesWithChecked(int[] rootIds, int[] checkedModuleIds)
        {
            var modules = _securityManager.Modules.Where(m => rootIds.Contains(m.Id)).OrderBy(m => m.OrderCode).Select(m => new
            {
                m.Id,
                m.Name,
                m.OrderCode,
                m.Remark,
                ChildIds = _securityManager.Modules.Where(n => n.ParentId == m.Id).OrderBy(n => n.OrderCode).Select(n => n.Id).ToList()
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
                    Items = item.ChildIds.Count > 0 ? GetModulesWithChecked(item.ChildIds.ToArray(), checkedModuleIds) : new List<object>()
                };

                if (node.Items.Count == 0 && !IsRoleLimit(node.Id))
                {
                    continue;
                }

                nodes.Add(node);
            }
            return nodes;
        }

        private bool IsRoleLimit(int moduleId)
        {
            return _securityManager.Functions
                .Where(m => _securityManager.ModuleFunctions.Where(n => n.ModuleId == moduleId).Select(n => n.FunctionId).Contains(m.Id))
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
            Expression<Func<Module, bool>> moduleExp = FilterHelper.GetExpression<Module>(request.FilterGroup);
            int[] moduleIds = _securityManager.Modules.Where(moduleExp).Select(m => m.Id).ToArray();
            Guid[] functionIds = _securityManager.ModuleFunctions.Where(m => moduleIds.Contains(m.ModuleId))
                .Select(m => m.FunctionId).Distinct().ToArray();
            if (functionIds.Length == 0)
            {
                return new PageData<FunctionOutputDto2>();
            }
            if (request.PageCondition.SortConditions.Length == 0)
            {
                request.PageCondition.SortConditions = new[] { new SortCondition("Area"), new SortCondition("Controller") };
            }
            var page = _securityManager.Functions.ToPage(m => functionIds.Contains(m.Id),
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
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("新增子节点")]
        public async Task<AjaxResult> Create(ModuleInputDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            OperationResult result = await _securityManager.CreateModule(dto);
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
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新")]
        public async Task<AjaxResult> Update(ModuleInputDto dto)
        {
            Check.NotNull(dto, nameof(dto));
            if (dto.Id == 1)
            {
                return new AjaxResult("根节点不能编辑", AjaxResultType.Error);
            }

            OperationResult result = await _securityManager.UpdateModule(dto);
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
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除")]
        public async Task<AjaxResult> Delete([FromForm] int id)
        {
            Check.NotNull(id, nameof(id));
            Check.GreaterThan(id, nameof(id), 0);
            if (id == 1)
            {
                return new AjaxResult("根节点不能删除", AjaxResultType.Error);
            }

            OperationResult result = await _securityManager.DeleteModule(id);
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
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("设置功能")]
        public async Task<AjaxResult> SetFunctions([FromBody] ModuleSetFunctionDto dto)
        {
            OperationResult result = await _securityManager.SetModuleFunctions(dto.ModuleId, dto.FunctionIds);
            return result.ToAjaxResult();
        }
        */
    }
}