// -----------------------------------------------------------------------
//  <copyright file="AssemblyExtensions.cs" company="柳柳软件">
//      Copyright (c) 2014 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-09-08 7:46</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.Versioning;

using Microsoft.Extensions.DependencyModel;

using OSharp.Data;
using OSharp.Extensions;


namespace OSharp.Reflection
{
    /// <summary>
    /// 程序集扩展操作类
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// 获取程序集的文件版本
        /// </summary>
        public static string GetFileVersion(this Assembly assembly)
        {
            assembly.CheckNotNull("assembly");
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return info.FileVersion;
        }

        /// <summary>
        /// 获取程序集的产品版本
        /// </summary>
        public static string GetProductVersion(this Assembly assembly)
        {
            assembly.CheckNotNull("assembly");
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = info.ProductVersion;
            if (version?.Contains("+") == true)
            {
                version = version.ReplaceRegex(@"\+(\w+)?", "");
            }
            return version;
        }

        /// <summary>
        /// 【仅支持Windows】获取一个正在运行的进程的命令行参数。
        /// 与<see cref="Environment.GetCommandLineArgs"/>一样，使用此方法获取的参数是包含应用程序启动时的参数
        /// </summary>
        /// <param name="process">一个正在运行的进程</param>
        /// <returns>表示应用程序运行命令行参数的字符串</returns>
#if !NETSTANDARD
        [SupportedOSPlatform("windows")]
#endif
        public static string GetCommandLineArgs(this Process process)
        {
            Check.NotNull(process, nameof(process));
            try
            {
                return GetCommandLineArgsCore();
            }
            catch (Win32Exception ex) when ((uint)ex.ErrorCode == 0x80004005)
            {
                return string.Empty;
            }
            catch (InvalidOperationException)
            {
                return string.Empty;
            }

            string GetCommandLineArgsCore()
            {
                using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
                {
                    using (var objects = searcher.Get())
                    {
                        var @object = objects.Cast<ManagementBaseObject>().SingleOrDefault();
                        return @object?["CommandLine"].ToString() ?? "";
                    }
                }
            }
        }

        /// <summary>
        /// 获取CLI版本号
        /// </summary>
        public static string GetCliVersion()
        {
            string[] dllNames =
            {
                "Microsoft.EntityFrameworkCore",
                "Microsoft.Extensions.Configuration.Binder",
                "Microsoft.Extensions.DependencyInjection",
                "Microsoft.Extensions.DependencyInjection.Abstractions",
                "Microsoft.Extensions.Configuration.Abstractions"
            };
            CompilationLibrary lib = null;
            foreach (string dllName in dllNames)
            {
                lib = DependencyContext.Default?.CompileLibraries.FirstOrDefault(m => m.Name == dllName);
                if (lib != null)
                {
                    break;
                }
            }
            string cliVersion = lib?.Version;
            return cliVersion;
        }
    }
}
