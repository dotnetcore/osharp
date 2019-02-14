// -----------------------------------------------------------------------
//  <copyright file="ISoftDeletable.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-02-14 22:44</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Entity
{
    /// <summary>
    /// 定义逻辑删除功能
    /// </summary>
    public interface ISoftDeletable
    {
        /// <summary>
        /// 获取或设置 是否逻辑删除
        /// </summary>
        bool IsDeleted { get; set; }
    }
}