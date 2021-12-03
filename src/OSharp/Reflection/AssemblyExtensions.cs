// -----------------------------------------------------------------------
//  <copyright file="AssemblyExtensions.cs" company="柳柳软件">
//      Copyright (c) 2014 66SOFT. All rights reserved.
//  </copyright>
//  <site>http://www.66soft.net</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014-09-08 7:46</last-date>
// -----------------------------------------------------------------------

using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyModel;

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
            if (version.Contains("+"))
            {
                version = version.ReplaceRegex(@"\+(\w+)?", "");
            }
            return version;
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
                lib = DependencyContext.Default.CompileLibraries.FirstOrDefault(m => m.Name == dllName);
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