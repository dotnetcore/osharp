// -----------------------------------------------------------------------
//  <copyright file="SmoothWeightSlaveDatabaseSelector.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-21 18:16</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Core.Options;


namespace OSharp.Entity
{
    /// <summary>
    /// 平滑权重从数据库选择器
    /// </summary>
    public sealed class WeightSlaveDatabaseSelector : ISlaveDatabaseSelector
    {
        private static readonly object LockObj = new object();
        private readonly ILogger _logger;
        private Queue<int> _queue = new Queue<int>();

        /// <summary>
        /// 初始化一个<see cref="WeightSlaveDatabaseSelector"/>类型的新实例
        /// </summary>
        public WeightSlaveDatabaseSelector(IServiceProvider provider)
        {
            _logger = provider.GetLogger(GetType());
        }

        /// <summary>
        /// 获取 名称
        /// </summary>
        public string Name => "Weight";

        /// <summary>
        /// 从所有从数据库中返回一个
        /// </summary>
        /// <param name="slaves">所有从数据库</param>
        /// <returns></returns>
        public SlaveDatabaseOptions Select(SlaveDatabaseOptions[] slaves)
        {
            lock (LockObj)
            {
                if (_queue.Count == 0)
                {
                    _queue = GetIndexes(slaves);
                }

                int index = _queue.Dequeue();
                SlaveDatabaseOptions slave = slaves[index];
                _logger.LogDebug($"平滑权重选取了“{slave.Name}”的从数据库，权重：{slave.Weight}");
                return slave;
            }
        }

        private static Queue<int> GetIndexes(SlaveDatabaseOptions[] slaves)
        {
            SlaveDatabaseOptionsWrap[] wraps = slaves.Select(m => new SlaveDatabaseOptionsWrap(m)).ToArray();
            int sum = wraps.Sum(m => m.Weight);
            Queue<int> queue = new Queue<int>();
            for (int i = 0; i < sum; i++)
            {
                int index = NextIndex(wraps);
                queue.Enqueue(index);
            }

            return queue;
        }

        private static int NextIndex(SlaveDatabaseOptionsWrap[] wraps)
        {
            int index = -1, total = 0;
            for (int i = 0; i < wraps.Length; i++)
            {
                SlaveDatabaseOptionsWrap wrap = wraps[i];
                wrap.Current += wrap.Weight;
                total += wrap.Weight;
                if (index == -1 || wraps[index].Current < wrap.Current)
                {
                    index = i;
                }
            }

            wraps[index].Current -= total;
            return index;
        }


        private class SlaveDatabaseOptionsWrap : SlaveDatabaseOptions
        {
            public SlaveDatabaseOptionsWrap(SlaveDatabaseOptions slave)
            {
                Weight = slave.Weight;
            }

            /// <summary>
            /// 获取或设置 当前权重
            /// </summary>
            public int Current { get; set; }
        }
    }
}