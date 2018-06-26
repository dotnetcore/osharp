// -----------------------------------------------------------------------
//  <copyright file="MapFromAttribute.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-14 0:25</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Data;


namespace OSharp.Mapping
{
    /// <summary>
    /// 标注当前类型从源类型的Mapping映射关系
    /// </summary>
    public class MapFromAttribute : Attribute
    {
        /// <summary>
        /// 初始化一个<see cref="MapFromAttribute"/>类型的新实例
        /// </summary>
        public MapFromAttribute(params Type[] sourceTypes)
        {
            Check.NotNull(sourceTypes, nameof(sourceTypes));
            SourceTypes = sourceTypes;
        }

        /// <summary>
        /// 源类型
        /// </summary>
        public Type[] SourceTypes { get; }
    }
}