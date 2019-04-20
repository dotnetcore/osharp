// -----------------------------------------------------------------------
//  <copyright file="IEntityHash.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-12 9:31</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Entity
{
    /// <summary>
    /// 定义实体Hash功能，对实体的属性值进行Hash，确定实体是否存在变化，
    /// 这些变化可用于系统初始化时确定是否需要进行数据同步
    /// </summary>
    public interface IEntityHash
    { }
}