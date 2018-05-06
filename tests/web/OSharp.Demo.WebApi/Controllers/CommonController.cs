// -----------------------------------------------------------------------
//  <copyright file="CommonController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-04 20:28</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-通用")]
    public class CommonController : ApiController
    {
        [Description("验证码")]
        public IActionResult VerifyCode()
        {
            return Json("");
        }
    }
}