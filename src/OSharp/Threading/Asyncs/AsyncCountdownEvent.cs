// -----------------------------------------------------------------------
//  <copyright file="AsyncCountdownEvent.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2016 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2016-03-31 23:08</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;


namespace OSharp.Threading.Asyncs
{
    public class AsyncCountdownEvent
    {
        private readonly AsyncManualResetEvent _amre = new AsyncManualResetEvent();
        private int _count;

        public AsyncCountdownEvent(int initialCount)
        {
            if (initialCount <= 0)
            {
                throw new ArgumentOutOfRangeException("initialCount");
            }
            _count = initialCount;
        }

        public Task WaitAsync()
        {
            return _amre.WaitAsync();
        }

        public void Signal()
        {
            if (_count <= 0)
            {
                throw new InvalidOperationException();
            }

            int newCount = Interlocked.Decrement(ref _count);
            if (newCount == 0)
            {
                _amre.Set();
            }
            else if (newCount < 0)
            {
                throw new InvalidOperationException();
            }
        }

        public Task SignalAndWait()
        {
            Signal();
            return WaitAsync();
        }
    }
}