// -----------------------------------------------------------------------
//  <copyright file="AsyncLock.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2016 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2016-03-31 20:48</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;


namespace OSharp.Threading.Asyncs
{
    /// <summary>
    /// 异步锁
    /// </summary>
    public class AsyncLock
    {
        private readonly Task<Releaser> _releaser;
        private readonly AsyncSemaphore _semaphore;

        /// <summary>
        /// 初始化一个<see cref="AsyncLock"/>类型的新实例
        /// </summary>
        public AsyncLock()
        {
            _semaphore = new AsyncSemaphore(1);
            _releaser = Task.FromResult(new Releaser(this));
        }

        /// <summary>
        /// 异步锁定
        /// </summary>
        public Task<Releaser> LockAsync()
        {
            var wait = _semaphore.WaitAsync();
            return wait.IsCompleted
                ? _releaser
                : wait.ContinueWith((_, state) => new Releaser((AsyncLock)state),
                    this,
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);
        }


        /// <summary>
        /// 释放资源的包装
        /// </summary>
        public struct Releaser : IDisposable
        {
            private readonly AsyncLock _toRelease;

            internal Releaser(AsyncLock toRelease)
            {
                _toRelease = toRelease;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <filterpriority>2</filterpriority>
            public void Dispose()
            {
                if (_toRelease != null)
                {
                    _toRelease._semaphore.Release();
                }
            }
        }
    }
}