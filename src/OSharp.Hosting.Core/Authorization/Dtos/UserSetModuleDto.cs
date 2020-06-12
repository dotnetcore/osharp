// -----------------------------------------------------------------------
//  <copyright file="UserSetModuleDto.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-08 12:21</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Hosting.Authorization.Dtos
{
    /// <summary>
    /// 用户设置模块DTO
    /// </summary>
    public class UserSetModuleDto
    {
        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 要设置的模块编号
        /// </summary>
        public int[] ModuleIds { get; set; }
    }
}