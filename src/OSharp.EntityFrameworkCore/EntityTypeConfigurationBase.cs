// -----------------------------------------------------------------------
//  <copyright file="EntityTypeConfigurationBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-17 0:40</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace OSharp.Entity
{
    /// <summary>
    /// 数据实体映射配置基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    public abstract class EntityTypeConfigurationBase<TEntity, TKey> : IEntityTypeConfiguration<TEntity>, IEntityRegister
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 获取 所属的上下文类型，如为null，将使用默认上下文， 否则使用指定类型的上下文类型
        /// </summary>
        public virtual Type DbContextType => null;

        /// <summary>
        /// 获取 相应的实体类型
        /// </summary>
        public Type EntityType => typeof(TEntity);

        /// <summary>
        /// 将当前实体类映射对象注册到数据上下文模型构建器中
        /// </summary>
        /// <param name="modelBuilder">上下文模型构建器</param>
        public void RegisterTo(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(this);
            // 给软删除的实体添加全局过滤器
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                modelBuilder.Entity<TEntity>().HasQueryFilter(m => ((ISoftDeletable)m).DeletedTime == null);
            }
        }

        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public abstract void Configure(EntityTypeBuilder<TEntity> builder);
    }
}
