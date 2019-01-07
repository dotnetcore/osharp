
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using OSharp.CodeGeneration.Schema;

using Shouldly;

using Xunit;

namespace OSharp.CodeGeneration.Tests
{
    public class RazorCodeGeneratorTests
    {
        private readonly ModuleMetadata _module;
        private readonly EntityMetadata _entity;

        public RazorCodeGeneratorTests()
        {
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
                    new PropertyMetadata() { Name = "IsHoted", Display = "是否热点", TypeName = typeof(bool).FullName },
                    new PropertyMetadata() { Name = "IsLocked", Display = "是否锁定", TypeName = typeof(bool).FullName },
                    new PropertyMetadata() { Name = "IsDeleted", Display = "是否删除", TypeName = typeof(bool).FullName },
                }
            };
        }

        [Fact]
        public void GenerateEntityCodeTest()
        {
            CodeFile code = new RazorCodeGenerator().GenerateEntityCode(_entity);
            AssertCodeFile(code);
        }

        [Fact]
        public void GenerateEntityConfigurationTest()
        {
            CodeFile code = new RazorCodeGenerator().GenerateEntityConfiguration(_entity);
            AssertCodeFile(code);
        }

        private void AssertCodeFile(CodeFile code)
        {
            code.FileName.ShouldBe("Liuliu.Site.EntityConfiguration/Infos/ArticleConfiguration.cs");
            code.SourceCode.ShouldContain("namespace");
            code.SourceCode.ShouldContain("public class");
        }
    }
}
