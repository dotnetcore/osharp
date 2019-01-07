
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using OSharp.CodeGeneration.Schema;
using OSharp.Extensions;

using Shouldly;

using Xunit;

namespace OSharp.CodeGeneration.Tests
{
    public class RazorCodeGeneratorTests
    {
        private readonly ModuleMetadata _module;
        private readonly EntityMetadata _entity;
        private readonly ICodeGenerator _generator;

        public RazorCodeGeneratorTests()
        {
            _generator = new RazorCodeGenerator();

            ProjectMetadata project = new ProjectMetadata()
            {
                Display = "XXX系统",
                Company = "柳柳软件",
                Copyright = "Copyright (c) 2014-2019 OSharp. All rights reserved.",
                Creator = "郭明锋",
                SiteUrl = "https://www.66soft.net",
                NamespacePrefix = "Liuliu.Site"
            };
            _module = new ModuleMetadata()
            {
                Name = "Infos",
                Display = "信息",
                Project = project
            };
            _entity = new EntityMetadata()
            {
                Name = "Article",
                Display = "文章",
                Module = _module,
                PrimaryKeyTypeFullName = typeof(int).FullName,
                Properties = new List<PropertyMetadata>()
                {
                    new PropertyMetadata() { Name = "Title", Display = "标题", TypeName = typeof(string).FullName },
                    new PropertyMetadata() { Name = "Content", Display = "正文", TypeName = typeof(string).FullName },
                    new PropertyMetadata() { Name = "IsHot", Display = "是否热点", TypeName = typeof(bool).FullName },
                    new PropertyMetadata() { Name = "IsLocked", Display = "是否锁定", TypeName = typeof(bool).FullName },
                    new PropertyMetadata() { Name = "IsDeleted", Display = "是否删除", TypeName = typeof(bool).FullName },
                }
            };
        }

        [Fact]
        public void GenerateEntityCodeTest()
        {
            CodeFile code = _generator.GenerateEntityCode(_entity);
            AssertCodeFile(code, "Liuliu.Site.Core/Infos/Entities/Article.cs");
        }

        [Fact]
        public void GenerateEntityConfigurationTest()
        {
            CodeFile code = _generator.GenerateEntityConfiguration(_entity);
            AssertCodeFile(code, "Liuliu.Site.EntityConfiguration/Infos/ArticleConfiguration.cs");
        }

        [Fact]
        public void GenerateServiceContractTest()
        {
            CodeFile code = _generator.GenerateServiceContract(_module);
            AssertCodeFile(code, "Liuliu.Site.Core/Infos/IInfosContract.cs");
        }

        [Fact]
        public void GenerateServiceMainImplTest()
        {
            CodeFile code = _generator.GenerateServiceMainImpl(_module);
            AssertCodeFile(code, "Liuliu.Site.Core/Infos/InfosService.cs");
        }

        [Fact]
        public void GenerateServiceEntityImplTest()
        {
            CodeFile code = _generator.GenerateServiceEntityImpl(_entity);
            AssertCodeFile(code, "Liuliu.Site.Core/Infos/InfosService.Article.cs");
        }

        private static void AssertCodeFile(CodeFile code, string checkFileName)
        {
            code.FileName.ShouldBe(checkFileName);
            code.SourceCode.ShouldContain("namespace");
            code.SourceCode.ShouldContain("using");
            code.SourceCode.ShouldContain("{");
            code.SourceCode.ShouldContain("}");
        }

    }
}
