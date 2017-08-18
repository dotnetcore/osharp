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
using System.Reflection;

namespace OSharp.Reflection
{
    /// <summary>
    /// Bin 目录程序集查找器
    /// </summary>
    public class BinAllAssemblyFinder : DirectoryAssemblyFinder, IAllAssemblyFinder
    {
        /// <summary>
        /// 初始化一个<see cref="BinAllAssemblyFinder"/>类型的新实例
        /// </summary>
        public BinAllAssemblyFinder()
            : base(GetBinPath())
        { }

        private static string GetBinPath()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string path1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
            if (path == path1)
            {
                return path;
            }
            return path == Environment.CurrentDirectory + "\\" ? path : Path.Combine(path, "bin");
        }
    }
}