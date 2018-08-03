// -----------------------------------------------------------------------
//  <copyright file="AreaApiController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-09 20:32</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc.Filters;


namespace OSharp.AspNetCore.Mvc
{
    /// <summary>
    /// WebApi的区域控制器基类
    /// </summary>
    [AuditOperation]
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public abstract class AreaApiController : Controller
    { }
}