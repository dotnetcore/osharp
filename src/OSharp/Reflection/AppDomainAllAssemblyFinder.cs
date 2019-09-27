// -----------------------------------------------------------------------
//  <copyright file="BinAssemblyFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-15 23:33</last-date>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyModel;

using OSharp.Collections;
using OSharp.Finders;

namespace OSharp.Reflection
{
    /// <summary>
    /// 应用程序目录程序集查找器
    /// </summary>
    public class AppDomainAllAssemblyFinder : FinderBase<Assembly>, IAllAssemblyFinder
    {
        private readonly bool _filterNetAssembly;

        /// <summary>
        /// 初始化一个<see cref="AppDomainAllAssemblyFinder"/>类型的新实例
        /// </summary>
        public AppDomainAllAssemblyFinder(bool filterNetAssembly = true)
        {
            _filterNetAssembly = filterNetAssembly;
        }

        /// <summary>
        /// 重写以实现程序集的查找
        /// </summary>
        /// <returns></returns>
        protected override Assembly[] FindAllItems()
        {
            string[] filters =
            {
                "mscorlib",
                "netstandard",
                "dotnet",
                "api-ms-win-core",
                "runtime.",
                "System",
                "Microsoft",
                "Window",
            };
            DependencyContext context = DependencyContext.Default;
            if (context != null)
            {
                List<string> names = new List<string>();
                string[] dllNames = context.CompileLibraries.SelectMany(m => m.Assemblies).Distinct().Select(m => m.Replace(".dll", ""))
                    .OrderBy(m => m).ToArray();
                if (dllNames.Length > 0)
                {
                    names = (from name in dllNames
                             let i = name.LastIndexOf('/') + 1
                             select name.Substring(i, name.Length - i)).Distinct()
                        .WhereIf(name => !filters.Any(name.StartsWith), _filterNetAssembly)
                        .OrderBy(m => m).ToList();
                }
                else
                {
                    foreach (CompilationLibrary library in context.CompileLibraries)
                    {
                        string name = library.Name;
                        if (_filterNetAssembly && filters.Any(name.StartsWith))
                        {
                            continue;
                        }
                        if (name == "OSharpNS")
                        {
                            continue;
                        }
                        if (name == "OSharpNS.Core")
                        {
                            name = "OSharp";
                        }
                        else if (name.StartsWith("OSharpNS."))
                        {
                            name = name.Replace("OSharpNS.", "OSharp.");
                        }
                        if (!names.Contains(name))
                        {
                            names.Add(name);
                        }
                    }
                }
                return LoadFiles(names);
            }

            //遍历文件夹的方式，用于传统.netfx
            string path = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly)
                .Concat(Directory.GetFiles(path, "*.exe", SearchOption.TopDirectoryOnly))
                .ToArray();

            return files.Where(file => filters.All(token => Path.GetFileName(file)?.StartsWith(token) != true)).Select(Assembly.LoadFrom).ToArray();
        }

        private static Assembly[] LoadFiles(IEnumerable<string> files)
        {
            List<Assembly> assemblies = new List<Assembly>();
            foreach (string file in files)
            {
                AssemblyName name = new AssemblyName(file);
                try
                {
                    assemblies.Add(Assembly.Load(name));
                }
                catch (FileNotFoundException)
                { }
            }
            return assemblies.ToArray();
        }
    }
}