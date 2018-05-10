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
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;

using OSharp.Collections;
using OSharp.Demo.Identity.Entities;
using OSharp.Demo.Security.Entities;
using OSharp.Entity;


namespace OSharp.Demo.WebApi.Controllers
{
    [Description("网站-测试")]
    public class TestController : ApiController
    {
        private readonly IRepository<ModuleFunction, Guid> _moduleFunctionRepository;
        private readonly IRepository<ModuleRole, Guid> _moduleRoleRepository;
        private readonly IRepository<Role, int> _roleRepository;

        public TestController(IRepository<ModuleFunction, Guid> moduleFunctionRepository,
            IRepository<ModuleRole, Guid> moduleRoleRepository,
            IRepository<Role, int> roleRepository)
        {
            _moduleFunctionRepository = moduleFunctionRepository;
            _moduleRoleRepository = moduleRoleRepository;
            _roleRepository = roleRepository;
        }

        [Description("测试01")]
        public IActionResult Test01(Guid functionId)
        {
            List<object> list = new List<object>();

            int[] moduleIds = _moduleFunctionRepository.Query(m => m.FunctionId == functionId).Select(m => m.ModuleId).ToArray();
            list.Add(moduleIds.ExpandAndToString());

            int[] roleIds = _moduleRoleRepository.Query(m => moduleIds.Contains(m.ModuleId)).Select(m => m.RoleId).ToArray();
            list.Add(roleIds.ExpandAndToString());

            return Content(list.ExpandAndToString("\r\n"));
        }
    }
}