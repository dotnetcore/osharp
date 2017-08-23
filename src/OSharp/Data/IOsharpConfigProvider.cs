// -----------------------------------------------------------------------
//  <copyright file="IOsharpConfigProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-23 13:14</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Data
{
    /// <summary>
    /// 定义OSharp配置信息提供者
    /// </summary>
    public interface IOsharpConfigProvider
    {
        /// <summary>
        /// 创建OSharp配置信息
        /// </summary>
        /// <returns></returns>
        OsharpConfig Create();
    }
}