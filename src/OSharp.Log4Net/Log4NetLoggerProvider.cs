// -----------------------------------------------------------------------
//  <copyright file="Log4NetLoggerProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-30 0:16</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;


namespace OSharp.Log4Net
{
    /// <summary>
    /// log4net 日志对象提供者
    /// </summary>
    public class Log4NetLoggerProvider : Disposable, ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new ConcurrentDictionary<string, Log4NetLogger>();
        private const string DefaultLog4NetFileName = "log4net.config";
        private readonly ILoggerRepository _loggerRepository;

        /// <summary>
        /// 初始化一个<see cref="Log4NetLoggerProvider"/>类型的新实例
        /// </summary>
        public Log4NetLoggerProvider() : this(DefaultLog4NetFileName)
        { }

        /// <summary>
        /// 初始化一个<see cref="Log4NetLoggerProvider"/>类型的新实例
        /// </summary>
        public Log4NetLoggerProvider(string log4NetConfigFile)
        {
            string file = log4NetConfigFile ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultLog4NetFileName);
            Assembly assembly = Assembly.GetEntryAssembly() ?? GetCallingAssemblyFromStartup();
            _loggerRepository = LogManager.CreateRepository(assembly, typeof(Hierarchy));

            if (File.Exists(file))
            {
                XmlConfigurator.ConfigureAndWatch(_loggerRepository, new FileInfo(file));
                return;
            }
            RollingFileAppender appender = new RollingFileAppender
            {
                Name = "root",
                File = "log\\log_",
                AppendToFile = true,
                LockingModel = new FileAppender.MinimalLock(),
                RollingStyle = RollingFileAppender.RollingMode.Date,
                DatePattern = "yyyyMMdd-HH\".log\"",
                StaticLogFileName = false,
                MaxSizeRollBackups = 10,
                Layout = new PatternLayout("[%d{HH:mm:ss.fff}] %-5p %c T%t:%n%m%n")
            };
            appender.ClearFilters();
            appender.AddFilter(new LevelMatchFilter() { LevelToMatch = Level.Debug });
            BasicConfigurator.Configure(_loggerRepository, appender);
            appender.ActivateOptions();
        }

        /// <summary>
        /// 创建一个 <see cref="T:Microsoft.Extensions.Logging.ILogger" /> 的新实例
        /// </summary>
        /// <param name="categoryName">记录器生成的消息的类别名称。</param>
        /// <returns>日志实例</returns>
        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, key => new Log4NetLogger(_loggerRepository.Name, key));
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