// -----------------------------------------------------------------------
//  <copyright file="ViewModelLocator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-07 22:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using OSharp.CodeGeneration.Generates;
using OSharp.CodeGeneration.Services;
using OSharp.CodeGeneration.Services.Entities;
using OSharp.CodeGenerator.Views.Modules;
using OSharp.CodeGenerator.Views.Projects;
using OSharp.Wpf.Stylet;


namespace OSharp.CodeGenerator.Views
{
    public class ViewModelLocator
    {
        /// <summary>
        /// 获取 实体主键类型数据源
        /// </summary>
        public string[] EntityKeys { get; } =
        {
            typeof(int).FullName,
            typeof(Guid).FullName,
            typeof(string).FullName,
            typeof(long).FullName
        };

        public string[] TypeNames { get; } =
        {
            typeof(string).FullName,
            typeof(int).FullName,
            typeof(bool).FullName,
            typeof(double).FullName,
            typeof(DateTime).FullName,
            typeof(Guid).FullName,
            typeof(long).FullName,
            "ICollection<>"
        };

        public ForeignRelation[] ForeignRelations { get; } = { ForeignRelation.ManyToOne, ForeignRelation.OneToMany, ForeignRelation.OneToOne, ForeignRelation.OwnsOne, ForeignRelation.OwnsMany };

        public DeleteBehavior?[] DeleteBehaviors { get; } = { null, DeleteBehavior.ClientSetNull, DeleteBehavior.Restrict, DeleteBehavior.SetNull, DeleteBehavior.Cascade };

        public MetadataType[] MetadataTypes { get; } = { MetadataType.Entity, MetadataType.Module, MetadataType.Project };

        public string[] TemplateFiles { get; } = { "内置" };

        public string[] DataAuthFlags { get; } = { "UserFlag" };

        public string[] Entities => GetEntities();

        private CodeProject _project;
        private string[] GetEntities()
        {
            ProjectViewModel project = IoC.Get<MenuViewModel>().Project;
            ModuleListViewModel moduleList = IoC.Get<ModuleListViewModel>();
            List<string> list = new List<string>()
            {
                "OSharp.Core.EntityInfos.EntityInfo",
                "OSharp.Core.Functions.Function",
                "OSharp.Core.Systems.KeyValue"
            };
            list.AddRange(new[]
            {
                "Identity.Entities.LoginLog",
                "Identity.Entities.Organization",
                "Identity.Entities.Role",
                "Identity.Entities.RoleClaim",
                "Identity.Entities.User",
                "Identity.Entities.UserClaim",
                "Identity.Entities.UserDetail",
                "Identity.Entities.UserLogin",
                "Identity.Entities.UserRole",
                "Identity.Entities.UserToken",
                "Security.Entities.EntityRole",
                "Security.Entities.EntityUser",
                "Security.Entities.Module",
                "Security.Entities.ModuleFunction",
                "Security.Entities.ModuleRole",
                "Security.Entities.ModuleUser",
                "Systems.Entities.AuditEntity",
                "Systems.Entities.AuditOperation",
                "Systems.Entities.AuditProperty"
            }.Select(m => $"{project.NamespacePrefix}.{m}"));
            if (moduleList != null)
            {
                if (_project == null)
                {
                    IServiceProvider rootProvider = IoC.Get<IServiceProvider>();
                    MenuViewModel menu = rootProvider.GetRequiredService<MenuViewModel>();
                    rootProvider.ExecuteScopedWork(provider =>
                    {
                        IDataContract contract = provider.GetRequiredService<IDataContract>();
                        _project = contract.GetCodeProject(m => m.Name == menu.Project.Name).First();
                    });
                }
                list.AddRange(_project.Modules.SelectMany(m => m.Entities.Select(n => $"{project.NamespacePrefix}.{m.Name}.Entities.{n.Name}"))
                    .OrderBy(m => m));
            }

            return list.Distinct().ToArray();
        }
    }
}
