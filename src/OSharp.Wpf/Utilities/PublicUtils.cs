// -----------------------------------------------------------------------
//  <copyright file="HttpHelper.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-26 17:30</last-date>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.Win32;

using OSharp.Data;


namespace OSharp.Wpf.Utilities
{
    public static class PublicUtils
    {
        /// <summary>
        /// 获取 主窗口句柄
        /// </summary>
        public static IntPtr ShellHwnd => Process.GetCurrentProcess().MainWindowHandle;

        /// <summary>
        /// 使用默认浏览器打开指定Url
        /// </summary>
        public static void OpenUrl(string url)
        {
            Check.NotNull(url, nameof(url));
            RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
            string value = registryKey?.GetValue("").ToString();
            if (value != null)
            {
                Process.Start(value.Substring(0, value.Length - 8), url);
            }
        }

        /// <summary>
        /// 窗口闪烁
        /// </summary>
        public static void FlashWindow(int count = 6, int interval = 500)
        {
            Task.Factory.StartNew(() =>
            {
                User32.FLASHWINFO pwfi = User32.FLASHWINFO.Create();
                pwfi.hwnd = ShellHwnd;
                pwfi.dwFlags = User32.FlashWindowFlags.FLASHW_ALL | User32.FlashWindowFlags.FLASHW_TIMERNOFG;
                pwfi.uCount = 6;
                pwfi.dwTimeout = 500;
                User32.FlashWindowEx(ref pwfi);
            });
        }

        /// <summary>
        /// 使用管理员权限运行指定程序
        /// </summary>
        public static Process RunAsAdministrator(string path)
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            //判断当前用户是否管理员
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                return Process.Start(path);
            }

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = path, 
                UseShellExecute = true, 
                WorkingDirectory = Environment.CurrentDirectory, 
                Verb = "runas"
            };
            try
            {
                return Process.Start(info);
            }
            catch
            {
                return null;
            }
        }
    }
}