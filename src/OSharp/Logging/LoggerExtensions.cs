// -----------------------------------------------------------------------
//  <copyright file="LoggerExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-06-02 2:40</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.Extensions.Logging;


namespace OSharp.Logging
{
    /// <summary>
    /// <see cref="ILogger"/>扩展方法
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// 记录异常日志信息，并返回异常以便抛出
        /// </summary>
        public static Exception LogExceptionMessage(this ILogger logger, Exception exception, string message = null)
        {
            logger.LogError(exception, message ?? exception.Message);
            return exception;
        }
    }
}