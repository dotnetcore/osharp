// -----------------------------------------------------------------------
//  <copyright file="DirectoryAssemblyFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-15 23:31</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;


namespace OSharp.Reflection
{
    /// <summary>
    /// 目录程序集查找器
    /// </summary>
    public class DirectoryAssemblyFinder : IAssemblyFinder
    {
        private static readonly ConcurrentDictionary<string, Assembly[]> CacheDict = new ConcurrentDictionary<string, Assembly[]>();
        private readonly string _path;
        
        /// <summary>
        /// 初始化一个<see cref="DirectoryAssemblyFinder"/>类型的新实例
        /// </summary>
        public DirectoryAssemblyFinder(string path)
        {
            _path = path;
        }

        /// <summary>
        /// 查找指定条件的项
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <param name="fromCache">是否来自缓存</param>
        /// <returns></returns>
        public Assembly[] Find(Func<Assembly, bool> predicate, bool fromCache = false)
        {
            return FindAll(fromCache).Where(predicate).ToArray();
        }

        /// <summary>
        /// 查找所有项
        /// </summary>
        /// <returns></returns>
        public Assembly[] FindAll(bool fromCache = false)
        {
            if (fromCache && CacheDict.ContainsKey(_path))
            {
                return CacheDict[_path];
            }
            string[] files = Directory.GetFiles(_path, "*.dll", SearchOption.TopDirectoryOnly)
                .Concat(Directory.GetFiles(_path, "*.exe", SearchOption.TopDirectoryOnly))
                .ToArray();
            Assembly[] assemblies = files.Select(Assembly.LoadFrom).Distinct().ToArray();
            CacheDict[_path] = assemblies;
            return assemblies;
        }

    }
}