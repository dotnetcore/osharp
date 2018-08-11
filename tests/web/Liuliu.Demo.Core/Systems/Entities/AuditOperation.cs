// -----------------------------------------------------------------------
//  <copyright file="AuditOperation.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 4:07</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.AspNetCore.UI;
using OSharp.Audits;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Mapping;


namespace Liuliu.Demo.Systems.Entities
{
    /// <summary>
    /// 实体类：审计操作信息
    /// </summary>
    [MapFrom(typeof(AuditOperationEntry))]
    [Description("审计操作信息")]
    public class AuditOperation : EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 执行的功能名
        /// </summary>
        [Required, DisplayName("执行的功能名")]
        public string FunctionName { get; set; }

        /// <summary>
        /// 获取或设置 当前用户标识
        /// </summary>
        [DisplayName("当前用户标识")]
        public string UserId { get; set; }

        /// <summary>
        /// 获取或设置 操作者用户名
        /// </summary>
        [DisplayName("操作者用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 操作者昵称
        /// </summary>
        [DisplayName("操作者昵称")]
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置 当前访问IP
        /// </summary>
        [DisplayName("当前访问IP")]
        public string Ip { get; set; }

        /// <summary>
        /// 获取或设置 操作系统
        /// </summary>
        [DisplayName("操作系统")]
        public string OperationSystem { get; set; }

        /// <summary>
        /// 获取或设置 浏览器
        /// </summary>
        [DisplayName("浏览器")]
        public string Browser { get; set; }

        /// <summary>
        /// 获取或设置 当前访问UserAgent
        /// </summary>
        [DisplayName("用户代理")]
        public string UserAgent { get; set; }

        /// <summary>
        /// 获取或设置 操作结果
        /// </summary>
        [DisplayName("操作结果")]
        public AjaxResultType ResultType { get; set; } = AjaxResultType.Success;

        /// <summary>
        /// 获取或设置 结果消息
        /// </summary>
        [DisplayName("结果消息")]
        public string Message { get; set; }

        /// <summary>
        /// 获取或设置 执行耗时，单位毫秒
        /// </summary>
        [DisplayName("执行耗时")]
        public int Elapsed { get; set; }

        /// <summary>
        /// 获取或设置 信息添加时间
        /// </summary>
        [DisplayName("添加时间")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 获取或设置 审计实体信息集合
        /// </summary>
        public virtual ICollection<AuditEntity> AuditEntities { get; set; } = new List<AuditEntity>();
    }
}