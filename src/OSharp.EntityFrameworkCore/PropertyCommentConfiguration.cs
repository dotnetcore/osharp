// -----------------------------------------------------------------------
//  <copyright file="EntityPropertyCommentConfiguration.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-18 0:27</last-date>
// -----------------------------------------------------------------------

using System.Linq;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// 给实体属性添加Comment配置
    /// 生成数据库时将属性[DisplayName]特性的值添加到表字段的描述信息中
    /// </summary>
    public class PropertyCommentConfiguration : IEntityBatchConfiguration
    {
        /// <summary>
        /// 配置指定的<see cref="IMutableEntityType"/>
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        /// <param name="mutableEntityType">实体的<see cref="IMutableEntityType"/>类型</param>
        public void Configure(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
        {
            IMutableProperty[] mutableProperties = mutableEntityType.GetProperties().ToArray();
            foreach (IMutableProperty mutableProperty in mutableProperties)
            {
                if (mutableProperty.PropertyInfo == null)
                {
                    continue;
                }

                string display = mutableProperty.PropertyInfo.GetDescription();
                modelBuilder.Entity(mutableEntityType.ClrType).Property(mutableProperty.PropertyInfo.Name).HasComment(display);
            }
        }
    }
}