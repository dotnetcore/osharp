// -----------------------------------------------------------------------
//  <copyright file="IMapTuple.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-25 1:04</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Mapping
{
    /// <summary>
    /// 定义对象映射源与目标配对
    /// </summary>
    public interface IMapTuple
    {
        /// <summary>
        /// 执行对象映射构造
        /// </summary>
        void CreateMap();
    }
}