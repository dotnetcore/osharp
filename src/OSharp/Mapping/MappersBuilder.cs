// -----------------------------------------------------------------------
//  <copyright file="MappersBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-26 13:56</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;

using OSharp.Dependency;


namespace OSharp.Mapping
{
    /// <summary>
    /// 对象映射构造器
    /// </summary>
    public class MappersBuilder : IMappersBuilder, ISingletonDependency
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