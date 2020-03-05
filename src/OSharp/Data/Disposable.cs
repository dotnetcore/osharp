// -----------------------------------------------------------------------
//  <copyright file="Disposable.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-05 18:05</last-date>
// -----------------------------------------------------------------------

namespace System
{
    /// <summary>
    /// <see cref="IDisposable"/>接口的一般实现
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        /// <summary>
        /// 这个实例被处理了吗？
        /// </summary>
        protected bool Disposed { get; private set; }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disposed = true; 
            }
        }

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        ~Disposable()
        {
            Dispose(false);
        }
    }
}