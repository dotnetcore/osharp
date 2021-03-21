// -----------------------------------------------------------------------
//  <copyright file="IMasterSlaveSplitPolicy.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-21 12:34</last-date>
// -----------------------------------------------------------------------

using OSharp.Core.Options;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义数据库主从分离策略
    /// </summary>
    public interface IMasterSlaveSplitPolicy
    {
        /// <summary>
        /// 是否前往从数据库
        /// </summary>
        /// <param name="options">数据上下文选项</param>
        /// <returns></returns>
        bool IsToSlaveDatabase(OsharpDbContextOptions options);
    }
}