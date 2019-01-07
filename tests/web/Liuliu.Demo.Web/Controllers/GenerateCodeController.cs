// -----------------------------------------------------------------------
//  <copyright file="GenerateCodeController.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-06 23:09</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

using OSharp.CodeGeneration;
using OSharp.CodeGeneration.Schema;
using OSharp.Core.Modules;


namespace Liuliu.Demo.Web.Controllers
{
    [Description("网站-生成代码")]
    [ModuleInfo(Order = 4)]
    public class GenerateCodeController : Controller
    {
        private readonly ICodeGenerator _generator;

        public GenerateCodeController(ICodeGenerator generator)
        {
            _generator = generator;
        }

        [HttpGet]
        [Description("生成代码")]
        public ActionResult Generate()
        {
            string code = null;//_generator.Generate(generateContext);

            return Content(code);
        }
    }
}