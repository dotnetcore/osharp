// -----------------------------------------------------------------------
//  <copyright file="SequenceSlaveDatabaseSelector.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-21 0:35</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Core.Options;


namespace OSharp.Entity
{
    /// <summary>
    /// 顺序轮询从数据库选择器
    /// </summary>
    public sealed class SequenceSlaveDatabaseSelector : ISlaveDatabaseSelector
    {
        private static readonly object LockObj = new object();
        private int _sequenceIndex;
        private readonly ILogger _logger;

        /// <summary>
        /// 初始化一个<see cref="SequenceSlaveDatabaseSelector"/>类型的新实例
        /// </summary>
        public SequenceSlaveDatabaseSelector(IServiceProvider provider)
        {
            _logger = provider.GetLogger(GetType());
        }

        /// <summary>
        /// 获取 名称
        /// </summary>
        public string Name => "Sequence";

        /// <summary>
        /// 从所有从数据库中返回一个
        /// </summary>
        /// <param name="slaves">所有从数据库</param>
        /// <returns></returns>
        public SlaveDatabaseOptions Select(SlaveDatabaseOptions[] slaves)
        {
            lock (LockObj)
            {
                if (_sequenceIndex > slaves.Length - 1)
                {
                    _sequenceIndex = 0;
                }

                SlaveDatabaseOptions slave = slaves[_sequenceIndex];
                _logger.LogDebug($"顺序选取了“{slave.Name}”的从数据库，顺序号：{_sequenceIndex}");
                _sequenceIndex++;

                return slave;
            }
        }
    }
}