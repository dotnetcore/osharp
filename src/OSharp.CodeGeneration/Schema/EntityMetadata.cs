// -----------------------------------------------------------------------
//  <copyright file="EntityMetadata.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-06 12:25</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using OSharp.Exceptions;


namespace OSharp.CodeGeneration.Schema
{
    /// <summary>
    /// 实体元数据
    /// </summary>
    public class EntityMetadata
    {
        /// <summary>
        /// 获取或设置 类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 类型全名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 获取或设置 命名空间
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// 获取或设置 类型显示名称
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 获取或设置 所属模块信息
        /// </summary>
        public ModuleMetadata Module { get; set; }

        /// <summary>
        /// 获取或设置 实体属性元数据集合
        /// </summary>
        public ICollection<PropertyMetadata> PropertyMetadatas { get; set; } = new List<PropertyMetadata>();

        /// <summary>
        /// 获取主键属性元数据
        /// </summary>
        public PropertyMetadata GetPrimaryKey()
        {
            PropertyMetadata prop = PropertyMetadatas.FirstOrDefault(m => m.Name == "Id");
            if (prop == null)
            {
                throw new OsharpException($"实体类元数据“{Name}”中无法获取到主键属性元数据");
            }
            return prop;
        }
    }
}