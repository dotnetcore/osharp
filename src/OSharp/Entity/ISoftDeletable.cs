// -----------------------------------------------------------------------
//  <copyright file="ISoftDeletable.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-02-14 22:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义逻辑删除功能
    /// </summary>
    public interface ISoftDeletable
    {
        /// <summary>
        /// 获取或设置 数据逻辑删除时间，为null表示正常数据，有值表示已逻辑删除，同时删除时间每次不同也能保证索引唯一性
        /// </summary>
        [DisplayName("删除时间")]
        DateTime? DeletedTime { get; set; }
    }
}