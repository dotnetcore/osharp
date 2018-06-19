// -----------------------------------------------------------------------
//  <copyright file="ApiController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-04 20:30</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;

namespace OSharp.AspNetCore.Mvc
{
    /// <summary>
    /// WebApi控制器基类
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class ApiController : Controller
    { }
}