// -----------------------------------------------------------------------
//  <copyright file="ModuleController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-18 16:06</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.UI;
using OSharp.Data;
using OSharp.Demo.Security;
using OSharp.Demo.Security.Dtos;
using OSharp.Demo.Security.Entities;
using OSharp.Entity;
using OSharp.Filter;


namespace OSharp.Demo.Web.Areas.Admin.Controllers
{
    [Description("管理-模块信息")]
    [Area("Admin")]
    [Route("api/[area]/[controller]/[action]")]
    public class ModuleController : Controller
    {
        private readonly SecurityManager _securityManager;

        public ModuleController(SecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        [Description("读取")]
        public IActionResult Read()
        {
            ListFilterGroup group = new ListFilterGroup(Request);
            Expression<Func<Module, bool>> predicate = FilterHelper.GetExpression<Module>(group);
            var modules = _securityManager.Modules.Where(predicate).OrderBy(m => m.OrderCode).Select(m => new
            {
                m.Id,
                m.Name,
                m.ParentId,
                m.OrderCode,
                m.Remark
            }).ToList();
            return Json(modules);
        }

        [HttpPost]
        [Description("新增")]
        public async Task<IActionResult> Create(ModuleInputDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            OperationResult result = await _securityManager.CreateModule(dto);
            return Json(result.ToAjaxResult());
        }

        [HttpPost]
        [Description("更新")]
        public async Task<IActionResult> Update(ModuleInputDto dto)
        {
            Check.NotNull(dto, nameof(dto));
            if (dto.Id == 1)
            {
                return Json(new AjaxResult("根节点不能编辑", AjaxResultType.Warning));
            }

            OperationResult result = await _securityManager.UpdateModule(dto);
            return Json(result.ToAjaxResult());
        }

        [HttpPost]
        [Description("删除")]
        public async Task<IActionResult> Delete(int id)
        {
            Check.NotNull(id, nameof(id));
            if (id == 1)
            {
                return Json(new AjaxResult("根节点不能删除", AjaxResultType.Warning));
            }

            OperationResult result = await _securityManager.DeleteModule(id);
            return Json(result.ToAjaxResult());
        }
    }
}