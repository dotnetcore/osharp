// -----------------------------------------------------------------------
//  <copyright file="AuditPropertyEntry.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 4:26</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Audits
{
    /// <summary>
    /// 实体属性审计信息
    /// </summary>
    public class AuditPropertyEntry
    {
        /// <summary>
        /// 获取或设置 名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 获取或设置 字段
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 获取或设置 旧值
        /// </summary>
        public string OriginalValue { get; set; }

        /// <summary>
        /// 获取或设置 新值
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// 获取或设置 数据类型
        /// </summary>
        public string DataType { get; set; }
    }
}