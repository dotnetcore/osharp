// -----------------------------------------------------------------------
//  <copyright file="NLogLoggerProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>冰河之刃</last-editor>
//  <last-date>2019-08-27 16:08</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

using NLog;
using Microsoft.Extensions.Logging;
using System.Text;

using NLog.Config;
using NLog.Targets;
using LogLevel = NLog.LogLevel;

namespace OSharp.NLog
{
    /// <summary>
    /// NLog 日志对象提供者
    /// </summary>
    public class NLogLoggerProvider : Disposable, ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, NLogLogger> _loggers = new ConcurrentDictionary<string, NLogLogger>();
        private const string DefaultNLogFileName = "nlog.config";

        /// <summary>
        /// 初始化一个<see cref="NLogLoggerProvider"/>类型的新实例
        /// </summary>
        public NLogLoggerProvider() : this(DefaultNLogFileName)
        { }

        /// <summary>
        /// 初始化一个<see cref="NLogLoggerProvider"/>类型的新实例
        /// </summary>
        public NLogLoggerProvider(string nlogConfigFile)
        {
            string file = nlogConfigFile ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultNLogFileName);
            //Assembly assembly = Assembly.GetEntryAssembly() ?? GetCallingAssemblyFromStartup();

            if (File.Exists(file))
            {
                LogManager.LoadConfiguration(file);
                return;
            }
            //如果没有找到配置文件，则默认写一个保存目标，保存为文件。
            FileTarget appender = new FileTarget
            {
                Encoding = Encoding.UTF8,
                Header = "-------------------------------------------",//加一个日志开头横线，方便区分
                Name = "root",
                FileName = "log\\log_",
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                ConcurrentWrites = true,
                KeepFileOpen = false,
                ArchiveDateFormat = "yyyyMMdd-HH",
                MaxArchiveFiles = 10,
                Layout = "${longdate}|${logger}|${callsite}|${threadid}|${message}"
            };
            //更多Layout参数请参考网址 https://nlog-project.org/config/?tab=layout-renderers

            LoggingConfiguration loggingConfiguration = new LoggingConfiguration();
            loggingConfiguration.AddTarget(appender);

            loggingConfiguration.LoggingRules.Clear();
            //最小级别和最大级别，最小记录Debug级别的日志
            loggingConfiguration.AddRule(LogLevel.Debug, LogLevel.Error, appender);
        }

        /// <summary>
        /// 创建一个 <see cref="T:Microsoft.Extensions.Logging.ILogger" /> 的新实例
        /// </summary>
        /// <param name="categoryName">记录器生成的消息的类别名称。</param>
        /// <returns>日志实例</returns>
        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, key => new NLogLogger(key));
        }

        private static Assembly GetCallingAssemblyFromStartup()
        {
            var stackTrace = new System.Diagnostics.StackTrace(2);
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var frame = stackTrace.GetFrame(i);
                var type = frame.GetMethod()?.DeclaringType;

                if (string.Equals(type?.Name, "Startup", StringComparison.OrdinalIgnoreCase))
                {
                    return type?.Assembly;
                }
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                _loggers.Clear();
            }

            base.Dispose(disposing);
        }
    }
}