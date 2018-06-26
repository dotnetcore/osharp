// -----------------------------------------------------------------------
//  <copyright file="SettingsController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.Core.Modules;


namespace Liuliu.Demo.WebApi.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 1, Position = "System")]
    [Description("管理-系统设置")]
    public class SettingsController : AdminApiController
    { }
}