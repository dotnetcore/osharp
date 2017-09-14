// -----------------------------------------------------------------------
//  <copyright file="EntityInfoBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-14 15:28</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

using OSharp.Entity;
using OSharp.Reflection;


namespace OSharp.Infrastructure
{
    /// <summary>
    /// 实体信息基类
    /// </summary>
    public abstract class EntityInfoBase : EntityBase<Guid>, IEntityInfo
    {
        /// <summary>
        /// 获取或设置 实体属性信息Json字符串
        /// </summary>
        public string PropertyNamesJson { get; set; }

        /// <summary>
        /// 获取或设置 实体名称
        /// </summary>
        [Required, DisplayName("实体名称")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 实体类型名称
        /// </summary>
        [Required, DisplayName("实体类型名称")]
        public string ClassFullName { get; set; }

        /// <summary>
        /// 获取或设置 是否启用数据日志
        /// </summary>
        [DisplayName("是否启用数据日志")]
        public bool DataLogEnabled { get; set; } = true;

        /// <summary>
        /// 获取 实体属性信息字典
        /// </summary>
        public IDictionary<string, string> PropertyNames
        {
            get
            {
                if (PropertyNamesJson.IsNullOrEmpty())
                {
                    return new Dictionary<string, string>();
                }
                return PropertyNamesJson.FromJsonString<Dictionary<string, string>>();
            }
        }

        /// <summary>
        /// 从实体类型初始化实体信息
        /// </summary>
        /// <param name="entityType"></param>
        public virtual void FromType(Type entityType)
        {
            Check.NotNull(entityType, nameof(entityType));

            ClassFullName = entityType.FullName;
            Name = entityType.GetDescription();
            DataLogEnabled = true;

            IDictionary<string, string> propertyDict = new Dictionary<string, string>();
            PropertyInfo[] propertyInfos = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                propertyDict.Add(propertyInfo.Name, propertyInfo.GetDescription());
            }
            PropertyNamesJson = propertyDict.ToJsonString();
        }
    }
}