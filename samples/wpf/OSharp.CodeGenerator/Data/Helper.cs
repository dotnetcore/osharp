// -----------------------------------------------------------------------
//  <copyright file="Helper.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-07 19:29</last-date>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Notifications.Wpf.Core;

using OSharp.CodeGenerator.Views;
using OSharp.Data;
using OSharp.Wpf.Stylet;


namespace OSharp.CodeGenerator.Data
{
    public static class Helper
    {
        public static MainView Main { get; set; }

        /// <summary>
        /// 输出信息到状态栏
        /// </summary>
        /// <param name="message">消息</param>
        public static void Output(string message)
        {
            IoC.Get<StatusBarViewModel>().Message = message;
        }

        /// <summary>
        /// 消息提示
        /// </summary>
        public static async void Notify(OperationResult result)
        {
            await NotifyAsync(result);
        }

        /// <summary>
        /// 消息提示
        /// </summary>
        public static async void Notify<T>(OperationResult<T> result)
        {
            await NotifyAsync(result);
        }

        /// <summary>
        /// 消息提示
        /// </summary>
        public static Task NotifyAsync(OperationResult result)
        {
            NotificationType type;
            switch (result.ResultType)
            {
                case OperationResultType.NoChanged:
                    type = NotificationType.Information;
                    break;
                case OperationResultType.Success:
                    type = NotificationType.Success;
                    break;
                default:
                    type = NotificationType.Error;
                    break;
            }
            return NotifyAsync(result.Message, type);
        }

        /// <summary>
        /// 消息提示
        /// </summary>
        public static Task NotifyAsync<T>(OperationResult<T> result)
        {
            NotificationType type;
            switch (result.ResultType)
            {
                case OperationResultType.NoChanged:
                    type = NotificationType.Information;
                    break;
                case OperationResultType.Success:
                    type = NotificationType.Success;
                    break;
                default:
                    type = NotificationType.Error;
                    break;
            }
            return NotifyAsync(result.Message, type);
        }

        /// <summary>
        /// 消息提示
        /// </summary>
        public static async void Notify(string message, NotificationType type, string title = "消息提示", Action onClick = null)
        {
            await NotifyAsync(message, type, title, onClick);
        }
        
        /// <summary>
        /// 消息提示
        /// </summary>
        public static Task NotifyAsync(string message, NotificationType type, string title = "消息提示", Action onClick = null)
        {
            MainViewModel main = IoC.Get<MainViewModel>();
            return main.Notify(message, type, title, onClick);
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        public static void OpenFolder(string path)
        {
            Process.Start("explorer.exe", path);
        }
    }
}
