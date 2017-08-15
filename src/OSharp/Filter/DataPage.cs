// -----------------------------------------------------------------------
//  <copyright file="DataPage.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2015-08-28 1:18</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Filter
{
    /// <summary>
    /// 数据分页信息
    /// </summary>
    public class PageResult<T>
    {
        /// <summary>
        /// 获取或设置 分页数据
        /// </summary>
        public T[] Data { get; set; }

        /// <summary>
        /// 获取或设置 总记录数
        /// </summary>
        public int Total { get; set; }
    }
}