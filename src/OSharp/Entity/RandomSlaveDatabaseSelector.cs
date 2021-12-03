// -----------------------------------------------------------------------
//  <copyright file="RandomSlaveDatabaseSelector.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-21 0:20</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Core.Options;
using OSharp.Extensions;


namespace OSharp.Entity
{
    /// <summary>
    /// 随机从数据库选择器
    /// </summary>
    public sealed class RandomSlaveDatabaseSelector : ISlaveDatabaseSelector
    {
        private static readonly Random Random = new Random();
        private readonly ILogger _logger;

        /// <summary>
        /// 初始化一个<see cref="RandomSlaveDatabaseSelector"/>类型的新实例
        /// </summary>
        public RandomSlaveDatabaseSelector(IServiceProvider provider)
        {
            _logger = provider.GetLogger(GetType());
        }

        /// <summary>
        /// 获取 名称
        /// </summary>
        public string Name => "Random";

        /// <summary>
        /// 从所有从数据库中返回一个
        /// </summary>
        /// <param name="slaves">所有从数据库</param>
        /// <returns></returns>
        public SlaveDatabaseOptions Select(SlaveDatabaseOptions[] slaves)
        {
            SlaveDatabaseOptions slave = Random.NextItem(slaves);
            _logger.LogDebug($"随机选取了“{slave.Name}”的从数据库");
            return slave;
        }
    }
}