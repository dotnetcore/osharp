// -----------------------------------------------------------------------
//  <copyright file="ModuleConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:48</last-date>
// -----------------------------------------------------------------------

using Liuliu.Demo.Security.Entities;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Entity;


namespace Liuliu.Demo.EntityConfiguration.Security
{
    /// <summary>
    /// 模块信息映射配置类
    /// </summary>
    public class ModuleConfiguration : EntityTypeConfigurationBase<Module, int>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasOne<Module>().WithMany().HasForeignKey(m => m.ParentId).IsRequired(false);

            builder.HasData(
                new Module() { Id = 1, Name = "根节点", Remark = "系统根节点", Code = "Root", OrderCode = 1, TreePathString = "$1$" }
                //new Module() { Id = 2, Name = "网站", Remark = "网站前台", Code = "Site", OrderCode = 1, ParentId = 1, TreePathString = "$1$,$2$" },
                //new Module() { Id = 3, Name = "管理", Remark = "管理后台", Code = "Admin", OrderCode = 2, ParentId = 1, TreePathString = "$1$,$3$" }
            );
        }
    }
}