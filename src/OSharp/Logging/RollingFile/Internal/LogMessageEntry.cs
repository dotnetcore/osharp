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
    public class LogMessageEntry
    {
        public DateTimeOffset Timestamp { get; set; }

        public string Message { get; set; }
    }
}