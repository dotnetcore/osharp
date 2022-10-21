// -----------------------------------------------------------------------
//  <copyright file="SyncLocker.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-12-20 18:16</last-date>
// -----------------------------------------------------------------------

using System;
using System.Threading;

using OSharp.Extensions;


namespace OSharp.Utility.Reflection
{
    /// <summary>
    /// 同步锁
    /// </summary>
    public static class SyncLocker
    {
        /// <summary>
        /// Mutex同步锁
        /// </summary>
        /// <param name="key">同步键的字符串，null时表示局部互斥体，否则根据process参数确定为Global\开头的进程锁或Local\开头的线程锁</param>
        /// <param name="action">要执行的业务操作</param>
        /// <param name="recursive">指示当前调用是否为递归处理，递归处理时检测到异常则抛出异常，避免进入无限递归</param>
        /// <param name="process">是否进程锁，true时，返回Global\开头，否则返回Local\开头</param>
        public static void MutexLock(string key, Action action, bool recursive = false, bool process = false)
        {
            string mutexKey = GetKey(key, process);
            using (Mutex mutex = new Mutex(false, mutexKey))
            {
                try
                {
                    mutex.WaitOne();
                    action();
                }
                catch (AbandonedMutexException)
                {
                    //当其他进程已上锁且没有正常释放互斥锁时(譬如进程忽然关闭或退出)，则会抛出AbandonedMutexException异常
                    if (recursive)
                    {
                        throw;
                    }
                    MutexLock(key, action, true, process);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        /// <summary>
        /// 操作系统级的同步键
        /// (如果将 name 指定为 null 或空字符串，则创建一个局部互斥体。 
        /// 如果名称以前缀“Global\”开头，则 mutex 在所有终端服务器会话中均为可见。 
        /// 如果名称以前缀“Local\”开头，则 mutex 仅在创建它的终端服务器会话中可见。 
        /// 如果创建已命名 mutex 时不指定前缀，则它将采用前缀“Local\”。)
        /// </summary>
        /// <param name="key">同步键的字符串，null时表示局部互斥体</param>
        /// <param name="process">是否进程锁，true时，返回Global\开头，否则返回Local\开头</param>
        /// <returns></returns>
        private static string GetKey(string key, bool process)
        {
            if (key.IsNullOrEmpty())
            {
                return null;
            }
            key = key.ToBase64String();
            return process ? $@"Global\{key}" : $@"Local\{key}";
        }
    }
}