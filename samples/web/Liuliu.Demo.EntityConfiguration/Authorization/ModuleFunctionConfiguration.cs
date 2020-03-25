// -----------------------------------------------------------------------
//  <copyright file="ModuleFunctionConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:48</last-date>
// -----------------------------------------------------------------------

using System;

using Liuliu.Demo.Authorization.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OSharp.Authorization.Functions;
using OSharp.Entity;


namespace Liuliu.Demo.EntityConfiguration.Authorization
{
    /// <summary>
    /// 模块功能信息映射配置类
    /// </summary>
    public partial class ModuleFunctionConfiguration : EntityTypeConfigurationBase<ModuleFunction, Guid>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<ModuleFunction> builder)
        {
            builder.HasIndex(m => new { m.ModuleId, m.FunctionId }).HasName("ModuleFunctionIndex").IsUnique();

            builder.HasOne<Module>(mf => mf.Module).WithMany().HasForeignKey(m => m.ModuleId);
            builder.HasOne<Function>(mf => mf.Function).WithMany().HasForeignKey(m => m.FunctionId);

            EntityConfigurationAppend(builder);
        }

        /// <summary>
        /// 额外的数据映射
        /// </summary>
        partial void EntityConfigurationAppend(EntityTypeBuilder<ModuleFunction> builder);
    }
}