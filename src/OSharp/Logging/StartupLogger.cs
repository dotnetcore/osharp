// -----------------------------------------------------------------------
//  <copyright file="StartupLog.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-08-09 12:45</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace OSharp.Logging
{
    /// <summary>
    /// 系统启动日志
    /// </summary>
    public class StartupLogger
    {
        public IList<LogInfo> LogInfos { get; } = new List<LogInfo>();

        public void LogInformation(string message, string logName)
        {
            Log(LogLevel.Information, message, logName);
        }

        public void LogDebug(string message, string logName)
        {
            Log(LogLevel.Debug, message, logName);
        }

        public void Log(LogLevel logLevel, string message, string logName, Exception exception = null)
        {
            LogInfo log = new LogInfo() { LogLevel = logLevel, Message = message, LogName = logName, Exception = exception, CreatedTime = DateTime.Now };
            LogInfos.Add(log);
        }

        public void Output(IServiceProvider provider)
        {
            IDictionary<string, ILogger> dict = new Dictionary<string, ILogger>();
            foreach (LogInfo info in LogInfos.OrderBy(m => m.CreatedTime))
            {
                if (!dict.TryGetValue(info.LogName, out ILogger logger))
                {
                    logger = provider.GetLogger(info.LogName);
                    dict[info.LogName] = logger;
                }

                switch (info.LogLevel)
                {
                    case LogLevel.Trace:
                        logger.LogTrace(info.Message);
                        break;
                    case LogLevel.Debug:
                        logger.LogDebug(info.Message);
                        break;
                    case LogLevel.Information:
                        logger.LogInformation(info.Message);
                        break;
                    case LogLevel.Warning:
                        logger.LogWarning(info.Message);
                        break;
                    case LogLevel.Error:
                        logger.LogError(info.Exception, info.Message);
                        break;
                    case LogLevel.Critical:
                        logger.LogCritical(info.Exception, info.Message);
                        break;
                }
            }
        }

        public class LogInfo
        {
            public LogLevel LogLevel { get; set; }

            public string Message { get; set; }

            public Exception Exception { get; set; }

            public string LogName { get; set; }

            public DateTime CreatedTime { get; set; }
        }
    }
}