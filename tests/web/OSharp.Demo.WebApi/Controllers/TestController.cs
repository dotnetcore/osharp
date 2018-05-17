// -----------------------------------------------------------------------
//  <copyright file="TestController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-10 22:46</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using OSharp.Collections;
using OSharp.Demo.Identity.Entities;
using OSharp.Demo.Security.Entities;
using OSharp.Entity;


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-测试")]
    public class TestController : ApiController
    {
        private readonly IRepository<EntityRole, Guid> _entityRoleRepository;

        public TestController(IRepository<EntityRole, Guid> entityRoleRepository)
        {
            _entityRoleRepository = entityRoleRepository;
        }

        [Description("测试01")]
        public IActionResult Test01(Guid functionId)
        {
            List<object> list = new List<object>();

            list.Add(_entityRoleRepository.Query().Count());

            return Content(list.ExpandAndToString("\r\n"));
        }
    }
}