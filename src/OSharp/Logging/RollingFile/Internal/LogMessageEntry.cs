// -----------------------------------------------------------------------
//  <copyright file="LogMessageEntry.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor></last-editor>
//  <last-date>2017-09-17 21:10</last-date>
// -----------------------------------------------------------------------

using System;


//power by https://github.com/andrewlock/NetEscapades.Extensions.Logging
namespace OSharp.Logging.RollingFile.Internal
{
    /// <summary>
    /// 日志消息项
    /// </summary>
    public class LogMessageEntry
    {
        /// <summary>
        /// 获取或设置 时间戳
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// 获取或设置 消息
        /// </summary>
        public string Message { get; set; }
    }
}