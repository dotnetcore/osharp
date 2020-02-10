// -----------------------------------------------------------------------
//  <copyright file="DataAuthOperation.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-14 11:18</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;


namespace OSharp.Authorization
{
    /// <summary>
    /// 数据权限操作
    /// </summary>
    public enum DataAuthOperation
    {
        /// <summary>
        /// 读取
        /// </summary>
        [Description("读取")]
        Read,

        /// <summary>
        /// 更新
        /// </summary>
        [Description("更新")]
        Update,

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete
    }
}