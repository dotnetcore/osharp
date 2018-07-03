// -----------------------------------------------------------------------
//  <copyright file="EntityProperty.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-04 4:34</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.EntityInfos
{
    /// <summary>
    /// 实体属性信息
    /// </summary>
    public class EntityProperty
    {
        /// <summary>
        /// 获取或设置 属性名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 属性显示
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// 获取或设置 属性数据类型
        /// </summary>
        public string TypeName { get; set; }
    }
}