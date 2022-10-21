// -----------------------------------------------------------------------
//  <copyright file="ThreadExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>最后修改人</last-editor>
//  <last-date>2014-07-29 2:57</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading;


namespace OSharp.Threading
{
    /// <summary>
    /// 线程扩展操作类
    /// </summary>
    public static class ThreadExtensions
    {
        /// <summary>
        /// 取消Thread.Sleep状态，继续线程
        /// </summary>
        public static void CancelSleep(this Thread thread)
        {
            if (thread.ThreadState != ThreadState.WaitSleepJoin)
            {
                return;
            }
            thread.Interrupt();
        }

        /// <summary>
        /// 启动线程，自动忽略停止线程时触发的<see cref="ThreadAbortException"/>异常
        /// </summary>
        /// <param name="thread">线程</param>
        /// <param name="failAction">引发非<see cref="ThreadAbortException"/>异常时执行的逻辑</param>
        public static void StartAndIgnoreAbort(this Thread thread, Action<Exception> failAction = null)
        {
            try
            {
                thread.Start();
            }
            catch (ThreadAbortException)
            { }
            catch (Exception e)
            {
                if (failAction != null)
                {
                    failAction(e);
                }
            }
        }
    }
}