// -----------------------------------------------------------------------
//  <copyright file="IUpdateAudited.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-04-12 10:27</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义更新审计的信息
    /// </summary>
    public interface IUpdateAudited<TUserKey> where TUserKey : struct
    {
        /// <summary>
        /// 获取或设置 更新者编号
        /// </summary>
        TUserKey? LastUpdaterId { get; set; }

        /// <summary>
        /// 获取或设置 最后更新时间
        /// </summary>
        DateTime? LastUpdatedTime { get; set; }
    }
}