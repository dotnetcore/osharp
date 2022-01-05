// -----------------------------------------------------------------------
//  <copyright file="CodeTemplateSeedDataInitializer.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-09 16:02</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq.Expressions;

using OSharp.CodeGeneration.Generates;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.Entity;


namespace OSharp.CodeGeneration.Services.Seeds
{
    public class CodeTemplateSeedDataInitializer : SeedDataInitializerBase<CodeTemplate, Guid>
    {
        /// <summary>
        /// 初始化一个<see cref="T:OSharp.Entity.SeedDataInitializerBase`2" />类型的新实例
        /// </summary>
        public CodeTemplateSeedDataInitializer(IServiceProvider rootProvider)
            : base(rootProvider)
        { }

        /// <summary>获取 种子数据初始化的顺序</summary>
        public override int Order => 1;

        /// <summary>重写以提供要初始化的种子数据</summary>
        /// <returns></returns>
        protected override CodeTemplate[] SeedData(IServiceProvider provider)
        {
            return new[]
            {
                new CodeTemplate(){Name = "cs_实体类", MetadataType = MetadataType.Entity, TemplateFile = "内置", Order = 1, IsSystem = true, OutputFileFormat = "{Project.NamespacePrefix}.Core/{Module.Name}/Entities/{Entity.Name}.generated.cs"},
                new CodeTemplate(){Name = "cs_输入DTO类", MetadataType = MetadataType.Entity, TemplateFile = "内置", Order = 2, IsSystem = true, OutputFileFormat = "{Project.NamespacePrefix}.Core/{Module.Name}/Dtos/{Entity.Name}InputDto.generated.cs"},
                new CodeTemplate(){Name = "cs_输出DTO类", MetadataType = MetadataType.Entity, TemplateFile = "内置", Order = 3, IsSystem = true, OutputFileFormat = "{Project.NamespacePrefix}.Core/{Module.Name}/Dtos/{Entity.Name}OutputDto.generated.cs"},
                new CodeTemplate(){Name = "cs_模块Pack类", MetadataType = MetadataType.Module, TemplateFile = "内置", Order = 4, IsSystem = true, IsOnce = true, OutputFileFormat = "{Project.NamespacePrefix}.Core/{Module.Name}/{Module.Name}Pack.cs"},
                new CodeTemplate(){Name = "cs_模块服务契约接口", MetadataType = MetadataType.Module, TemplateFile = "内置", Order = 5, IsSystem = true, OutputFileFormat = "{Project.NamespacePrefix}.Core/{Module.Name}/I{Module.Name}Contract.generated.cs"},
                new CodeTemplate(){Name = "cs_模块服务综合实现基类", MetadataType = MetadataType.Module, TemplateFile = "内置", Order = 6, IsSystem = true, OutputFileFormat = "{Project.NamespacePrefix}.Core/{Module.Name}/{Module.Name}ServiceBase.generated.cs"},
                new CodeTemplate(){Name = "cs_模块服务综合实现类", MetadataType = MetadataType.Module, TemplateFile = "内置", Order = 7, IsSystem = true, IsOnce = true, OutputFileFormat = "{Project.NamespacePrefix}.Core/{Module.Name}/{Module.Name}Service.cs"},
                new CodeTemplate(){Name = "cs_模块服务实体实现基类", MetadataType = MetadataType.Entity, TemplateFile = "内置", Order = 8, IsSystem = true, OutputFileFormat = "{Project.NamespacePrefix}.Core/{Module.Name}/{Module.Name}ServiceBase.{Entity.Name}.generated.cs"},
                new CodeTemplate(){Name = "cs_实体数据映射配置类", MetadataType = MetadataType.Entity, TemplateFile = "内置", Order = 9, IsSystem = true, OutputFileFormat = "{Project.NamespacePrefix}.EntityConfiguration/{Module.Name}/{Entity.Name}Configuration.generated.cs"},
                new CodeTemplate(){Name = "cs_实体管理控制器基类", MetadataType = MetadataType.Entity, TemplateFile = "内置", Order = 10, IsSystem = true, OutputFileFormat = "{Project.NamespacePrefix}.Web/Areas/Admin/Controllers/{Module.Name}/{Entity.Name}ControllerBase.generated.cs"},
                new CodeTemplate(){Name = "cs_实体管理控制器类", MetadataType = MetadataType.Entity, TemplateFile = "内置", Order = 11, IsSystem = true, IsOnce = true, OutputFileFormat = "{Project.NamespacePrefix}.Web/Areas/Admin/Controllers/{Module.Name}/{Entity.Name}Controller.cs"},
                new CodeTemplate(){Name = "ng_Alain模块", MetadataType = MetadataType.Module, TemplateFile = "内置", Order = 12, IsSystem = true, IsOnce = true, OutputFileFormat = "ui/ng-alain/src/app/routes/{Module.Name:Lower}/{Module.Name:Lower}.module.ts"},
                new CodeTemplate(){Name = "ng_Alain模块路由", MetadataType = MetadataType.Module, TemplateFile = "内置", Order = 13, IsSystem = true, IsOnce = true, OutputFileFormat = "ui/ng-alain/src/app/routes/{Module.Name:Lower}/{Module.Name:Lower}.routing.ts"},
                new CodeTemplate(){Name = "ng_Alain模块组件", MetadataType = MetadataType.Entity, TemplateFile = "内置", Order = 14, IsSystem = true, IsOnce = true, OutputFileFormat = "ui/ng-alain/src/app/routes/{Module.Name:Lower}/{Entity.Name:Lower}/{Entity.Name:Lower}.component.ts"},
                new CodeTemplate(){Name = "ng_Alain模块组件Html", MetadataType = MetadataType.Entity, TemplateFile = "内置", Order = 15, IsSystem = true, IsOnce = true, OutputFileFormat = "ui/ng-alain/src/app/routes/{Module.Name:Lower}/{Entity.Name:Lower}/{Entity.Name:Lower}.component.html"},
                new CodeTemplate(){Name = "ng_Alain其他数据", MetadataType = MetadataType.Project, TemplateFile = "内置", Order = 16, IsSystem = true, OutputFileFormat = "ui/ng-alain/src/assets/osharp/other.generated"},
                new CodeTemplate(){Name = "vue_Vben国际化多语言英文", MetadataType = MetadataType.Project, TemplateFile = "内置", Order = 17, IsSystem = true, OutputFileFormat = "ui/vue-vben/src/locales/lang/en/routes/osharp.ts"},
                new CodeTemplate(){Name = "vue_Vben国际化多语言简体中文", MetadataType = MetadataType.Project, TemplateFile = "内置", Order = 18, IsSystem = true, OutputFileFormat = "ui/vue-vben/src/locales/lang/zh-CN/routes/osharp.ts"},
                new CodeTemplate(){Name = "vue_Vben路由器路由", MetadataType = MetadataType.Project, TemplateFile = "内置", Order = 20, IsSystem = true, OutputFileFormat = "ui/vue-vben/src/router/routes/modules/osharp.ts"},
                new CodeTemplate(){Name = "vue_Vben存储Store", MetadataType = MetadataType.Entity, TemplateFile = "内置", Order = 21, IsSystem = true, OutputFileFormat = "ui/vue-vben/src/store/osharp/{Module.Name:Lower}/{Entity.Name:Lower}Store.ts"},
                new CodeTemplate(){Name = "vue_Vben视图Vue", MetadataType = MetadataType.Entity, TemplateFile = "内置", Order = 22, IsSystem = true, OutputFileFormat = "ui/vue-vben/src/views/osharp/{Module.Name:Lower}/{Entity.Name:Lower}.vue"},
            };
        }

        /// <summary>重写以提供判断某个实体是否存在的表达式</summary>
        /// <param name="entity">要判断的实体</param>
        /// <returns></returns>
        protected override Expression<Func<CodeTemplate, bool>> ExistingExpression(CodeTemplate entity)
        {
            return m => m.Name == entity.Name;
        }
    }
}
