// -----------------------------------------------------------------------
//  <copyright file="BinAssemblyFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-15 23:33</last-date>
// -----------------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Reflection;

using OSharp.Dependency;
using OSharp.Finders;

namespace OSharp.Reflection
{
    /// <summary>
    /// 应用程序目录程序集查找器
    /// </summary>
    public class AppAllAssemblyFinder : FinderBase<Assembly>, IAllAssemblyFinder, ISingletonDependency
    {
        private readonly bool _filterNetAssembly;

        /// <summary>
        /// 初始化一个<see cref="AppAllAssemblyFinder"/>类型的新实例
        /// </summary>
        public AppAllAssemblyFinder(bool filterNetAssembly = true)
        {
            _filterNetAssembly = filterNetAssembly;
        }

        /// <summary>
        /// 重写以获取应用程序目录
        /// </summary>
        /// <returns></returns>
        protected string GetAppPath()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            return path;
            //string path1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
            //if (path == path1)
            //{
            //    return path;
            //}
            //return path == Environment.CurrentDirectory + "\\" ? path : Path.Combine(path, "bin");
        }

        /// <summary>
        /// 重写以实现程序集的查找
        /// </summary>
        /// <returns></returns>
        protected override Assembly[] FindAllItems()
        {
            string path = GetAppPath();
            string[] files = Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly)
                .Concat(Directory.GetFiles(path, "*.exe", SearchOption.TopDirectoryOnly))
                .ToArray();
            if (_filterNetAssembly)
            {
                string[] filters = { "System.", "Microsoft.", "netstandard" };
                files = files.Where(m => files.Any(n => m.StartsWith(n, StringComparison.OrdinalIgnoreCase))).ToArray();
            }
            return files.Select(m => Assembly.LoadFrom(m)).ToArray();
        }
    }
}