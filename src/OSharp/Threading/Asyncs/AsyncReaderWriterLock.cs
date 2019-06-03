// -----------------------------------------------------------------------
//  <copyright file="AsyncReaderWriterLock.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2016 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2016-03-31 23:24</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace OSharp.Threading.Asyncs
{
    /// <summary>
    /// 异步读写锁
    /// </summary>
    public class AsyncReaderWriterLock
    {
        private readonly Task<Releaser> _readerReleaser;
        private readonly Queue<TaskCompletionSource<Releaser>> _waitingWriters = new Queue<TaskCompletionSource<Releaser>>();
        private readonly Task<Releaser> _writerReleaser;
        private int _readersWaiting;
        private int _status;
        private TaskCompletionSource<Releaser> _waitingReader =
            new TaskCompletionSource<Releaser>();

        public AsyncReaderWriterLock()
        {
            _readerReleaser = Task.FromResult(new Releaser(this, false));
            _writerReleaser = Task.FromResult(new Releaser(this, true));
        }

        public Task<Releaser> ReaderLockAsync()
        {
            lock (_waitingWriters)
            {
                if (_status >= 0 && _waitingWriters.Count == 0)
                {
                    ++_status;
                    return _readerReleaser;
                }

                ++_readersWaiting;
                return _waitingReader.Task.ContinueWith(t => t.Result);
            }
        }

        public Task<Releaser> WriterLockAsync()
        {
            lock (_waitingWriters)
            {
                if (_status == 0)
                {
                    _status = -1;
                    return _writerReleaser;
                }
                else
                {
                    var waiter = new TaskCompletionSource<Releaser>();
                    _waitingWriters.Enqueue(waiter);
                    return waiter.Task;
                }
            }
        }

        private void ReaderRelease()
        {
            TaskCompletionSource<Releaser> toWake = null;

            lock (_waitingWriters)
            {
                --_status;
                if (_status == 0 && _waitingWriters.Count > 0)
                {
                    _status = -1;
                    toWake = _waitingWriters.Dequeue();
                }
            }

            if (toWake != null)
            {
                toWake.SetResult(new Releaser(this, true));
            }
        }

        private void WriterRelease()
        {
            TaskCompletionSource<Releaser> toWake = null;
            bool toWakeIsWriter = false;

            lock (_waitingWriters)
            {
                if (_waitingWriters.Count > 0)
                {
                    toWake = _waitingWriters.Dequeue();
                    toWakeIsWriter = true;
                }
                else if (_readersWaiting > 0)
                {
                    toWake = _waitingReader;
                    _status = _readersWaiting;
                    _readersWaiting = 0;
                    _waitingReader = new TaskCompletionSource<Releaser>();
                }
                else
                {
                    _status = 0;
                }
            }

            if (toWake != null)
            {
                toWake.SetResult(new Releaser(this, toWakeIsWriter));
            }
        }


        public struct Releaser : IDisposable
        {
            private readonly AsyncReaderWriterLock _toRelease;
            private readonly bool _writer;

            internal Releaser(AsyncReaderWriterLock toRelease, bool writer)
            {
                _toRelease = toRelease;
                _writer = writer;
            }

            public void Dispose()
            {
                if (_toRelease != null)
                {
                    if (_writer)
                    {
                        _toRelease.WriterRelease();
                    }
                    else
                    {
                        _toRelease.ReaderRelease();
                    }
                }
            }
        }
    }
}