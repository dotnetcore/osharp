// -----------------------------------------------------------------------
//  <copyright file="SecurityController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-11 2:18</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc;


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-授权")]
    public class SecurityController : ApiController
    {
        [Description("判断URL授权")]
        public IActionResult CheckUrlAuth(string url)
        {
            bool ok = this.CheckFunctionAuth(url);
            return Json(ok);
        }

        [Description("获取授权信息")]
        public IActionResult AuthInfo()
        {









            return Json(null);
        }
    }
}