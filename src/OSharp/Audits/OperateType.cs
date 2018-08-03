// -----------------------------------------------------------------------
//  <copyright file="OperateType.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 3:59</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Audits
{
    /// <summary>
    /// 表示实体审计操作类型
    /// </summary>
    public enum OperateType
    {
        /// <summary>
        /// 查询
        /// </summary>
        Query = 0,
        /// <summary>
        /// 新增
        /// </summary>
        Insert = 1,
        /// <summary>
        /// 更新
        /// </summary>
        Update = 2,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 3
    }
}