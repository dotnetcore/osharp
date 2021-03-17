// -----------------------------------------------------------------------
//  <copyright file="AssemblyManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-17 11:22</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyModel;

using OSharp.Exceptions;


namespace OSharp.Reflection
{
    /// <summary>
    /// 程序集管理器
    /// </summary>
    public static class AssemblyManager
    {
        private static readonly string[] Filters = { "dotnet-", "Microsoft.", "mscorlib", "netstandard", "System", "Windows" };
        private static Assembly[] _allAssemblies;
        private static Type[] _allExportTypes;

        static AssemblyManager()
        {
            AssemblyFilterFunc = name =>
            {
                return name.Name != null && !Filters.Any(m => name.Name.StartsWith(m));
            };
        }

        /// <summary>
        /// 设置 程序集过滤器
        /// </summary>
        public static Func<AssemblyName, bool> AssemblyFilterFunc { private get; set; }

        /// <summary>
        /// 获取 所有程序集
        /// </summary>
        public static Assembly[] AllAssemblies
        {
            get
            {
                if (_allAssemblies == null)
                {
                    Init();
                }

                return _allAssemblies;
            }
        }

        /// <summary>
        /// 获取 所有公开类型
        /// </summary>
        public static Type[] AllExportTypes
        {
            get
            {
                if (_allExportTypes == null)
                {
                    Init();
                }

                return _allExportTypes;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            if (AssemblyFilterFunc == null)
            {
                throw new OsharpException("AssemblyManager.AssemblyFilterFunc 不能为空");
            }

            _allAssemblies = DependencyContext.Default.GetDefaultAssemblyNames()
                .Where(AssemblyFilterFunc).Select(Assembly.Load).ToArray();
            _allExportTypes = _allAssemblies.SelectMany(m => m.ExportedTypes).ToArray();
        }

        /// <summary>
        /// 查找指定条件的类型
        /// </summary>
        public static Type[] FindTypes(Func<Type, bool> predicate)
        {
            return AllExportTypes.Where(predicate).ToArray();
        }

        /// <summary>
        /// 查找指定基类的实现类型
        /// </summary>
        public static Type[] FindTypesByBase<TBaseType>()
        {
            Type baseType = typeof(TBaseType);
            return FindTypesByBase(baseType);
        }

        /// <summary>
        /// 查找指定基类的实现类型
        /// </summary>
        public static Type[] FindTypesByBase(Type baseType)
        {
            return AllExportTypes.Where(type => type.IsDeriveClassFrom(baseType)).Distinct().ToArray();
        }

        /// <summary>
        /// 查找指定Attribute特性的实现类型
        /// </summary>
        public static Type[] FindTypesByAttribute<TAttribute>(bool inherit = true)
        {
            Type attributeType = typeof(TAttribute);
            return FindTypesByAttribute(attributeType, inherit);
        }

        /// <summary>
        /// 查找指定Attribute特性的实现类型
        /// </summary>
        public static Type[] FindTypesByAttribute(Type attributeType, bool inherit = true)
        {
            return AllExportTypes.Where(type => type.IsDefined(attributeType, inherit)).Distinct().ToArray();
        }
    }
}