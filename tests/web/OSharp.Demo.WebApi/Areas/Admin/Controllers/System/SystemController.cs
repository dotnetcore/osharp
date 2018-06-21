// -----------------------------------------------------------------------
//  <copyright file="SystemController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-21 9:51</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using OSharp.Collections;
using OSharp.Core.Modules;
using OSharp.Entity;
using OSharp.Reflection;

namespace OSharp.Demo.WebApi.Areas.Admin.Controllers
{
    [Description("管理-系统")]
    public class SystemController : AdminApiController
    {
        private readonly IServiceProvider _provider;

        public SystemController(IServiceProvider provider)
        {
            _provider = provider;
        }
    }
}