// -----------------------------------------------------------------------
//  <copyright file="PageResult.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-01 20:43</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Filter
{
    /// <summary>
    /// 数据分页信息
    /// </summary>
    public class PageResult<T>
    {
        /// <summary>
        /// 初始化一个<see cref="PageResult{T}"/>类型的新实例
        /// </summary>
        public PageResult()
            : this(new T[0], 0)
        { }

        /// <summary>
        /// 初始化一个<see cref="PageResult{T}"/>类型的新实例
        /// </summary>
        public PageResult(T[] data, int total)
        {
            Data = data;
            Total = total;
        }

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