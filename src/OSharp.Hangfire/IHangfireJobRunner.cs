// -----------------------------------------------------------------------
//  <copyright file="IHangfireJobRunner.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-01-02 16:17</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Hangfire
{
    /// <summary>
    /// Hangfire作业运行器
    /// </summary>
    public interface IHangfireJobRunner
    {
        /// <summary>
        /// 开始运行
        /// </summary>
        void Start();
    }
}