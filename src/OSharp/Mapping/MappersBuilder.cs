// -----------------------------------------------------------------------
//  <copyright file="MappersBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-10-14 2:22</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;


namespace OSharp.Mapping
{
    /// <summary>
    /// 对象映射构造器
    /// </summary>
    public class MappersBuilder : IMappersBuilder
    {
        /// <summary>
        /// 执行对象映射构造
        /// </summary>
        /// <param name="mapTuples">对象映射源-目标查找器配对信息集合</param>
        public void Build(IEnumerable<IMapTuple> mapTuples)
        {
            foreach (IMapTuple mapTuple in mapTuples)
            {
                mapTuple.Build();
            }
        }
    }
}