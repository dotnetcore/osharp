// -----------------------------------------------------------------------
//  <copyright file="Id4EntityConfigurationBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-20 0:46</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Data;
using OSharp.Entity;


namespace OSharp.IdentityServer4.EntityFrameworkCore
{
    public abstract class Id4EntityTypeConfigurationBase<TEntity, TKey> : EntityTypeConfigurationBase<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 获取 所属的上下文类型，如为null，将使用默认上下文， 否则使用指定类型的上下文类型
        /// </summary>
        public override Type DbContextType
        {
            get
            {
                //todo: 这个配置不能信赖于DI，如何在初始化时读取配置？
                Type type = null;
                string typeName = Singleton<DbContextTypeOptions>.Instance?.DbContextTypeName;
                if (!string.IsNullOrWhiteSpace(typeName))
                {
                    type = Type.GetType(typeName.Trim());
                }
                return type;
            }
        }
    }
}