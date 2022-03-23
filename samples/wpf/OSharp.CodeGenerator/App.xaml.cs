using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Extensions.Logging;

using OSharp.Collections;
using OSharp.Dependency;
using OSharp.Wpf.Stylet;


namespace OSharp.CodeGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ILogger _logger;

        /// <summary>
        /// 初始化一个<see cref="App"/>类型的新实例
        /// </summary>
        public App()
        {
            //注册全局事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            const string msg = "主线程异常";
            try
            {
                if (args.ExceptionObject is Exception && Dispatcher != null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        Exception ex = (Exception)args.ExceptionObject;
                        HandleException(msg, ex);
                    });
                }
            }
            catch (Exception ex)
            {
                HandleException(msg, ex);
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            const string msg = "子线程异常";
            try
            {
                HandleException(msg, args.Exception);
                args.Handled = true;
            }
            catch (Exception ex)
            {
                HandleException(msg, ex);
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs args)
        {
            const string msg = "异步异常";
            try
            {
                HandleException(msg, args.Exception);
                args.SetObserved();
            }
            catch (Exception ex)
            {
                HandleException(msg, ex);
            }
        }

        private void HandleException(string msg, Exception ex)
        {
            _logger ??= IoC.Get<ILoggerFactory>().CreateLogger(typeof(App));

            List<string> lines = new List<string>();
            Exception innerEx = ex;
            while (innerEx != null)
            {
                lines.Add(innerEx.Message);
                innerEx = innerEx.InnerException;
            }

            string detail = lines.ExpandAndToString("\r\n---");
            msg = $"{msg}: {detail}";
            _logger?.LogCritical(ex, msg);
            MessageBox.Show($"错误消息：{detail}", "错误提示", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
