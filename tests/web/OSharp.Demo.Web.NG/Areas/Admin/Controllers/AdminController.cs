// -----------------------------------------------------------------------
//  <copyright file="AdminControllerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-01-14 22:38</last-date>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;


namespace OSharp.Demo.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]/[action]")]
    public abstract class AdminController : Controller
    { }
}