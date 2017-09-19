// -----------------------------------------------------------------------
//  <copyright file="AuditOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-17 23:15</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Audits
{
    /// <summary>
    /// 审计配置信息
    /// </summary>
    public class AuditOptions
    {
        /// <summary>
        /// 获取或设置 审计操作信息委托
        /// </summary>
        public Func<AuditOperation> OperationFactory { get; set; }

        /// <summary>
        /// 获取或设置 审计数据信息委托
        /// </summary>
        public Func<AuditEntity> DataFactory { get; set; }
    }
}