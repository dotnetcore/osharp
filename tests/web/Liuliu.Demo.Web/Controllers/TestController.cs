// -----------------------------------------------------------------------
//  <copyright file="TestController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:50</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Liuliu.Demo.Identity.Entities;
using Liuliu.Demo.Security.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using OSharp.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Collections;
using OSharp.Dependency;
using OSharp.Entity;
using OSharp.Identity;
using OSharp.Json;


namespace Liuliu.Demo.Web.Controllers
{
    [Description("网站-测试")]
    public class TestController : ApiController
    {
        [HttpGet]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("测试01")]
        public async Task<string> Test01()
        {
            List<object> list = new List<object>();

            IOnlineUserCache cache = ServiceLocator.Instance.GetService<IOnlineUserCache>();
            OnlineUser user = cache.GetOrRefresh("admin");
            list.Add(user.ToJsonString());

            return list.ExpandAndToString("\r\n");
        }
    }
}