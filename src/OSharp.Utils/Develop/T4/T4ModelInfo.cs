// -----------------------------------------------------------------------
//  <copyright file="T4ModelInfo.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-03-07 18:17</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using OSharp.Extensions;
using OSharp.Reflection;

namespace OSharp.Develop.T4
{
    /// <summary>
    /// T4实体模型信息类
    /// </summary>
    public class T4ModelInfo
    {
        /// <summary>
        /// 初始化一个<see cref="T4ModelInfo"/>类型的新实例
        /// </summary>
        /// <param name="modelType">实体类型</param>
        /// <param name="moduleNamePattern">模块名称正则表达式，用于从实体命名空间中提取模块名称</param>
        public T4ModelInfo(Type modelType, string moduleNamePattern = null)
        {
            modelType.CheckNotNull("modelType");
            string @namespace = modelType.Namespace;
            if (@namespace == null)
            {
                return;
            }
            Namespace = @namespace;
            if (moduleNamePattern != null)
            {
                ModuleName = @namespace.Match(moduleNamePattern);
            }
            Name = modelType.Name;
            Description = modelType.GetDescription();
            Properties = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo property = Properties.FirstOrDefault(m => m.HasAttribute<KeyAttribute>())
                ?? Properties.FirstOrDefault(m => m.Name.ToUpper() == "ID")
                    ?? Properties.FirstOrDefault(m => m.Name.ToUpper().EndsWith("ID"));
            if (property != null)
            {
                KeyType = property.PropertyType;
            }
        }

        /// <summary>
        /// 获取或设置 主键类型
        /// </summary>
        public Type KeyType { get; private set; }
        
        /// <summary>
        /// 获取 模型所在模块名称
        /// </summary>
        public string ModuleName { get; private set; }

        /// <summary>
        /// 获取 模型命名空间
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// 获取 模型名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取 模型描述
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// 属性信息集合
        /// </summary>
        public IEnumerable<PropertyInfo> Properties { get; private set; }

        /// <summary>
        /// 获取或设置 工程名称，生成代码的命名空间都基于此名称
        /// </summary>
        public string ProjectName { get; set; }
    }
}