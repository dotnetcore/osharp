// -----------------------------------------------------------------------
//  <copyright file="EntityMetadata.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-06 12:25</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

using OSharp.Reflection;


namespace OSharp.CodeGenerator
{
    /// <summary>
    /// 类型元数据
    /// </summary>
    public class TypeMetadata
    {
        /// <summary>
        /// 初始化一个<see cref="TypeMetadata"/>类型的新实例
        /// </summary>
        public TypeMetadata()
        { }

        /// <summary>
        /// 初始化一个<see cref="TypeMetadata"/>类型的新实例
        /// </summary>
        public TypeMetadata(Type type)
        {
            if (type == null)
            {
                return;
            }

            Name = type.Name;
            FullName = type.FullName;
            Namespace = type.Namespace;
            Display = type.GetDescription().Replace("信息", "");
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                if (property.HasAttribute<IgnoreGenPropertyAttribute>())
                {
                    continue;
                }
                if (property.GetMethod.IsVirtual && !property.GetMethod.IsFinal)
                {
                    continue;
                }
                if (PropertyMetadatas == null)
                {
                    PropertyMetadatas = new List<PropertyMetadata>();
                }
                PropertyMetadatas.Add(new PropertyMetadata(property));
            }
        }

        /// <summary>
        /// 获取或设置 类型名
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
        /// 获取或设置 类型显示名
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 获取或设置 属性元数据集合
        /// </summary>
        public IList<PropertyMetadata> PropertyMetadatas { get; set; }
    }
}