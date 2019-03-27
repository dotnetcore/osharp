// -----------------------------------------------------------------------
//  <copyright file="IEntityMapper.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-17 0:32</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义将实体配置类注册到上下文中
    /// </summary>
    public interface IEntityRegister
    {
        /// <summary>
        /// 获取所属的上下文类型，如为null，将使用默认上下文，否则使用指定类型的上下文类型
        /// </summary>
        Type DbContextType { get; }

        /// <summary>
        /// 获取 相应的实体类型
        /// </summary>
        Type EntityType { get; }

        /// <summary>
        /// 将当前实体类映射对象注册到数据上下文模型构建器中
        /// </summary>
        /// <param name="modelBuilder">上下文模型构建器</param>
        void RegisterTo(ModelBuilder modelBuilder);
    }
}