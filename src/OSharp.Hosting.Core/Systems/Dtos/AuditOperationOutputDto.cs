// -----------------------------------------------------------------------
//  <copyright file="AuditOperationOutputDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 15:43</last-date>
// -----------------------------------------------------------------------

using System;

using OSharp.Hosting.Systems.Entities;

using OSharp.Data;
using OSharp.Entity;
using OSharp.Mapping;


namespace OSharp.Hosting.Systems.Dtos
{
    /// <summary>
    /// 输入DTO：操作审计信息
    /// </summary>
    [MapFrom(typeof(AuditOperation))]
    public class AuditOperationOutputDto : IOutputDto
    {
        /// <summary>
        /// 获取或设置 数据编号
        /// </summary>
        public Guid Id { get; set; }

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
        /// 获取或设置 操作系统
        /// </summary>
        public string OperationSystem { get; set; }

        /// <summary>
        /// 获取或设置 浏览器
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 获取或设置 操作结果
        /// </summary>
        public AjaxResultType ResultType { get; set; } = AjaxResultType.Success;

        /// <summary>
        /// 获取或设置 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 获取或设置 执行耗时，单位毫秒
        /// </summary>
        public int Elapsed { get; set; }

        /// <summary>
        /// 获取或设置 信息添加时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }
}