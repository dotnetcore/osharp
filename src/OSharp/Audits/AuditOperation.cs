// -----------------------------------------------------------------------
//  <copyright file="AuditOperation.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-17 11:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;


namespace OSharp.Audits
{
    /// <summary>
    /// 审计操作信息
    /// </summary>
    public class AuditOperation
    {
        /// <summary>
        /// 初始化一个<see cref="AuditOperation"/>类型的新实例
        /// </summary>
        public AuditOperation()
        {
            AuditEntities = new List<AuditEntity>();
        }

        /// <summary>
        /// 获取或设置 执行的功能名
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// 获取或设置 当前用户标识
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 获取或设置 当前用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 当前用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置 当前访问IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 获取或设置 当前访问UserAgent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 获取或设置 信息添加时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 审计数据信息集合
        /// </summary>
        public ICollection<AuditEntity> AuditEntities { get; set; }
    }
}