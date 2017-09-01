// -----------------------------------------------------------------------
//  <copyright file="IMappersBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-26 13:55</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;


namespace OSharp.Mapping
{
    /// <summary>
    /// 定义对象映射构造器
    /// </summary>
    public interface IMappersBuilder
    {
        /// <summary>
        /// 执行对象映射构造
        /// </summary>
        /// <param name="mapTuples">对象映射源-目标查找器配对信息集合</param>
        void Build(IEnumerable<IMapTuple> mapTuples);
    }
}