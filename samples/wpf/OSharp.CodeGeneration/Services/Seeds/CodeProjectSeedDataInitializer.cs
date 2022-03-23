// -----------------------------------------------------------------------
//  <copyright file="CodeProjectSeedDataInitializer.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-09 16:56</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.Extensions.DependencyInjection;

using OSharp.CodeGeneration.Services.Dtos;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.Data;
using OSharp.Entity;


namespace OSharp.CodeGeneration.Services.Seeds
{
    public class CodeProjectSeedDataInitializer : SeedDataInitializerBase<CodeProject, Guid>
    {
        /// <summary>
        /// 初始化一个<see cref="T:OSharp.Entity.SeedDataInitializerBase`2" />类型的新实例
        /// </summary>
        public CodeProjectSeedDataInitializer(IServiceProvider rootProvider)
            : base(rootProvider)
        { }

        /// <summary>获取 种子数据初始化的顺序</summary>
        public override int Order => 2;

        /// <summary>重写以提供要初始化的种子数据</summary>
        /// <returns></returns>
        protected override CodeProject[] SeedData(IServiceProvider provider)
        {
            return new[]
            {
                new CodeProject()
                {
                    Name = "示例项目", NamespacePrefix = "Liuliu.Demo", Company = "LiuliuSoft", SiteUrl = "https://www.osharp.org", Creator = "郭明锋", Copyright = "OSHARP.ORG@2021"
                }
            };
        }

        /// <summary>重写以提供判断某个实体是否存在的表达式</summary>
        /// <param name="entity">要判断的实体</param>
        /// <returns></returns>
        protected override Expression<Func<CodeProject, bool>> ExistingExpression(CodeProject entity)
        {
            return m => m.Name == entity.Name;
        }

        /// <summary>将种子数据初始化到数据库</summary>
        protected override void SyncToDatabase(CodeProject[] entities, IServiceProvider provider)
        {
            if (entities == null || entities.Length == 0)
                return;
            IDataContract contract = provider.GetRequiredService<IDataContract>();
            var dtos = entities.Select(m => new CodeProjectInputDto()
            {
                Id = m.Id,
                Name = m.Name, NamespacePrefix = m.NamespacePrefix, Company = m.Company, Creator = m.Creator, Copyright = m.Copyright,
                RootPath = m.RootPath, SiteUrl = m.SiteUrl
            }).ToArray();
            contract.CreateCodeProjects(dtos).GetAwaiter().GetResult();
        }
    }
}
