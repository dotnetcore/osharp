// -----------------------------------------------------------------------
//  <copyright file="IKeyValue.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-25 20:52</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.Data
{
    /// <summary>
    /// 定义键值对数据
    /// </summary>
    public interface IKeyValueCouple
    {
        /// <summary>
        /// 获取或设置 数据键
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// 获取或设置 数据值
        /// </summary>
        object Value { get; set; }
    }
}