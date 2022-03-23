// -----------------------------------------------------------------------
//  <copyright file="CodeModuleSeedDataInitializer.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-09 17:01</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.Extensions.DependencyInjection;

using OSharp.CodeGeneration.Services.Entities;
using OSharp.Entity;


namespace OSharp.CodeGeneration.Services.Seeds
{
    public class CodeModuleSeedDataInitializer : SeedDataInitializerBase<CodeModule, Guid>
    {
        /// <summary>
        /// 初始化一个<see cref="T:OSharp.Entity.SeedDataInitializerBase`2" />类型的新实例
        /// </summary>
        public CodeModuleSeedDataInitializer(IServiceProvider rootProvider)
            : base(rootProvider)
        { }

        /// <summary>获取 种子数据初始化的顺序</summary>
        public override int Order => 3;

        /// <summary>重写以提供要初始化的种子数据</summary>
        /// <returns></returns>
        protected override CodeModule[] SeedData(IServiceProvider provider)
        {
            IRepository<CodeProject, Guid> repository = provider.GetRequiredService<IRepository<CodeProject, Guid>>();
            CodeProject project = repository.GetFirst(m => m.Name == "示例项目");
            return new[]
            {
                new CodeModule(){Name = "Identity", Display = "身份认证", Order = 1, ProjectId = project.Id},
                new CodeModule(){Name = "Auth", Display = "权限授权", Order = 2, ProjectId = project.Id},
                new CodeModule(){Name = "Infos", Display = "信息", Order = 3, ProjectId = project.Id},
            };
        }

        /// <summary>重写以提供判断某个实体是否存在的表达式</summary>
        /// <param name="entity">要判断的实体</param>
        /// <returns></returns>
        protected override Expression<Func<CodeModule, bool>> ExistingExpression(CodeModule entity)
        {
            return m => m.Name == entity.Name && m.ProjectId == entity.ProjectId;
        }
    }
}
