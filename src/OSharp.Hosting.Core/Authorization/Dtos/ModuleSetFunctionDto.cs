// -----------------------------------------------------------------------
//  <copyright file="ModuleSetFunctionDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-27 4:44</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Hosting.Authorization.Dtos
{
    /// <summary>
    /// 模块设置功能DTO
    /// </summary>
    public class ModuleSetFunctionDto
    {
        /// <summary>
        /// 获取或设置 模块编号
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// 获取或设置 功能编号集合
        /// </summary>
        public Guid[] FunctionIds { get; set; }
    }
}