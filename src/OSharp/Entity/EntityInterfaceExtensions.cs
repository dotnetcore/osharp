// -----------------------------------------------------------------------
//  <copyright file="EntityExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-19 22:15</last-date>
// -----------------------------------------------------------------------

using System;
using System.Security.Principal;

using OSharp.Data;
using OSharp.Identity;
using OSharp.Reflection;


namespace OSharp.Entity
{
    /// <summary>
    /// 实体接口扩展方法
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// 检测指定类型是否为<see cref="IEntity{TKey}"/>实体类型
        /// </summary>
        /// <param name="type">要判断的类型</param>
        /// <returns></returns>
        public static bool IsEntityType(this Type type)
        {
            Check.NotNull(type, nameof(type));
            return typeof(IEntity<>).IsGenericAssignableFrom(type) && !type.IsAbstract && !type.IsInterface;
        }

        /// <summary>
        /// 判断指定实体是否已过期
        /// </summary>
        /// <param name="entity">要检测的实体</param>
        /// <returns></returns>
        public static bool IsExpired(this IExpirable entity)
        {
            Check.NotNull(entity, nameof(entity));
            DateTime now = DateTime.Now;
            return entity.BeginTime != null && entity.BeginTime.Value > now || entity.EndTime != null && entity.EndTime.Value < now;
        }

        /// <summary>
        /// 检测并执行<see cref="ICreatedTime"/>接口的逻辑
        /// </summary>
        public static TEntity CheckICreatedTime<TEntity, TKey>(this TEntity entity)
            where TEntity : IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            if (!(entity is ICreatedTime))
            {
                return entity;
            }
            ICreatedTime entity1 = (ICreatedTime)entity;
            if (entity1.CreatedTime == default(DateTime))
            {
                entity1.CreatedTime = DateTime.Now;
            }

            return (TEntity)entity1;
        }

        /// <summary>
        /// 检测并执行<see cref="ICreationAudited{TUserKey}"/>接口的处理
        /// </summary>
        public static TEntity CheckICreationAudited<TEntity, TKey, TUserKey>(this TEntity entity, IPrincipal user)
            where TEntity : IEntity<TKey>
            where TKey : IEquatable<TKey>
            where TUserKey : struct, IEquatable<TUserKey>
        {
            if (!(entity is ICreationAudited<TUserKey>))
            {
                return entity;
            }

            ICreationAudited<TUserKey> entity1 = (ICreationAudited<TUserKey>)entity;
            entity1.CreatorId = user.Identity.IsAuthenticated ? (TUserKey?)user.Identity.GetUserId<TUserKey>() : null;
            if (entity1.CreatedTime == default(DateTime))
            {
                entity1.CreatedTime = DateTime.Now;
            }
            return (TEntity)entity1;
        }

        /// <summary>
        /// 检测并执行<see cref="IUpdateAudited{TUserKey}"/>接口的处理
        /// </summary>
        public static TEntity CheckIUpdateAudited<TEntity, TKey, TUserKey>(this TEntity entity, IPrincipal user)
            where TEntity : IEntity<TKey>
            where TKey : IEquatable<TKey>
            where TUserKey : struct, IEquatable<TUserKey>
        {
            if (!(entity is IUpdateAudited<TUserKey>))
            {
                return entity;
            }

            IUpdateAudited<TUserKey> entity1 = (IUpdateAudited<TUserKey>)entity;
            entity1.LastUpdaterId = user.Identity.IsAuthenticated ? (TUserKey?)user.Identity.GetUserId<TUserKey>() : null;
            entity1.LastUpdatedTime = DateTime.Now;
            return (TEntity)entity1;
        }
    }
}
