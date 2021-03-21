// -----------------------------------------------------------------------
//  <copyright file="SlaveDatabaseOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-20 10:22</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

using OSharp.Data;
using OSharp.Reflection;


namespace OSharp.Core.Options
{
    /// <summary>
    /// 从数据库选项
    /// </summary>
    [DebuggerDisplay("{Name} - {Weight}")]
    public class SlaveDatabaseOptions : DataErrorInfoBase
    {
        /// <summary>
        /// 获取或设置 数据库名
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "从数据库名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 权重
        /// </summary>
        [Range(1, 100, ErrorMessage = "权重值范围为1-100")]
        public int Weight { get; set; } = 1;

        /// <summary>
        /// 获取或设置 数据库连接串
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "从数据库连接串不能为空")]
        public string ConnectionString { get; set; }

        /// <summary>获取一条错误消息，指示此对象有什么问题。</summary>
        /// <returns>指示此对象存在什么问题的错误消息。默认值为空字符串（""）。</returns>
        public override string Error
        {
            get { return this["ConnectionString"]; }
        }
    }
}