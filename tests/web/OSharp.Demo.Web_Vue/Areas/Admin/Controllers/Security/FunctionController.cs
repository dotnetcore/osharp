// -----------------------------------------------------------------------
//  <copyright file="FunctionController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-11-04 15:40</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

using OSharp.Demo.Security;
using OSharp.Entity;
using OSharp.Filter;


namespace OSharp.Demo.Web.Areas.Admin.Controllers
{
    [Description("管理-权限安全")]
    [Area("Admin")]
    [Route("api/[area]/[controller]/[action]")]
    public class FunctionController : Controller
    {
        private readonly ISecurityContract _securityContract;

        public FunctionController(ISecurityContract securityContract)
        {
            _securityContract = securityContract;
        }

        [Description("读取")]
        public IActionResult Read()
        {
            var page = _securityContract.Functions.ToPage(m => true, 1, 10000, new SortCondition[0], m => new
            {
                Id = m.Id.ToString("N"),
                m.Name,
                m.Area,
                m.Controller,
                m.Action,
                m.IsController,
                m.IsAjax,
                m.AccessType,
                m.IsAccessTypeChanged,
                m.AuditOperationEnabled,
                m.AuditEntityEnabled,
                m.CacheExpirationSeconds,
                m.IsCacheSliding,
                m.IsLocked
            });
            return Json(page.ToPageData());
        }
    }
}