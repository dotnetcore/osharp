// -----------------------------------------------------------------------
//  <copyright file="AsyncSemaphore.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2016 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2016-03-31 20:36</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

using OSharp.Extensions;


namespace OSharp.Threading.Asyncs
{
    /// <summary>
    /// 异步信号量
    /// </summary>
    public class AsyncSemaphore
    {
        private static readonly Task Completed = Task.FromResult(true);
        private readonly Queue<TaskCompletionSource<bool>> _waiters = new Queue<TaskCompletionSource<bool>>();
        private int _currentCount;

        /// <summary>
        /// 初始化一个<see cref="AsyncSemaphore"/>类型的新实例
        /// </summary>
        public AsyncSemaphore(int initialCount)
        {
            initialCount.CheckGreaterThan("initialCount", 0);
            _currentCount = initialCount;
        }

        /// <summary>
        /// 等待同步
        /// </summary>
        public Task WaitAsync()
        {
            lock (_waiters)
            {
                if (_currentCount > 0)
                {
                    --_currentCount;
                    return Completed;
                }
                var waiter = new TaskCompletionSource<bool>();
                _waiters.Enqueue(waiter);
                return waiter.Task;
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Release()
        {
            TaskCompletionSource<bool> toRelease = null;
            lock (_waiters)
            {
                if (_waiters.Count > 0)
                {
                    toRelease = _waiters.Dequeue();
                }
                else
                {
                    ++_currentCount;
                }
            }
            if (toRelease != null)
            {
                toRelease.SetResult(true);
            }
        }
    }
}