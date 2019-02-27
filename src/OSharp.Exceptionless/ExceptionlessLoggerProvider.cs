// -----------------------------------------------------------------------
//  <copyright file="ExceptionlessLoggerProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-02-28 2:42</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.Logging;


namespace OSharp.Exceptionless
{
    /// <summary>
    /// Exceptionless日志对象提供者
    /// </summary>
    public class ExceptionlessLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new ExceptionlessLogger();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}