// -----------------------------------------------------------------------
//  <copyright file="AuditEntityController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 14:57</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

using OSharp.Core.Modules;


namespace Liuliu.Demo.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 3, Position = "System", PositionName = "系统管理模块")]
    [Description("管理-数据审计信息")]
    public class AuditEntityController : AdminApiController
    {
        [HttpGet]
        [Description("读取")]
        public IActionResult Read()
        {
            return Json("");
            
        }
    }
}