// -----------------------------------------------------------------------
//  <copyright file="AuditEntity.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-20 0:36</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using OSharp.Properties;


namespace OSharp.Audits
{
    /// <summary>
    /// 实体类：实体审计信息
    /// </summary>
    public class AuditEntity
    {
        /// <summary>
        /// 初始化一个<see cref="AuditEntity"/>类型的新实例
        /// </summary>
        public AuditEntity()
            : this(null, null, OperateType.Query)
        { }

        /// <summary>
        /// 初始化一个<see cref="AuditEntity"/>类型的新实例
        /// </summary>
        public AuditEntity(string name, string typeName, OperateType operateType)
        {
            Name = name;
            TypeName = typeName;
            OperateType = operateType;
            Properties = new List<AuditEntityProperty>();
        }

        /// <summary>
        /// 获取或设置 实体名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 获取或设置 数据编号
        /// </summary>
        public string EntityKey { get; set; }

        /// <summary>
        /// 获取或设置 操作类型
        /// </summary>
        public OperateType OperateType { get; set; }

        /// <summary>
        /// 获取或设置 操作实体属性集合
        /// </summary>
        public ICollection<AuditEntityProperty> Properties { get; set; }
    }


    /// <summary>
    /// 表示实体审计操作类型
    /// </summary>
    public enum OperateType
    {
        Query = 0,
        Insert = 1,
        Update = 2,
        Delete = 3
    }
}