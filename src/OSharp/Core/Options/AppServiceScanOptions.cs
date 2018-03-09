// -----------------------------------------------------------------------
//  <copyright file="AppServiceScanOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-03 13:39</last-date>
// -----------------------------------------------------------------------

using OSharp.Dependency;
using OSharp.Reflection;


namespace OSharp.Core.Options
{
    /// <summary>
    /// OSharp依赖注入类型扫描配置信息
    /// </summary>
    public class AppServiceScanOptions
    {
        /// <summary>
        /// 初始化一个<see cref="AppServiceScanOptions"/>类型的新实例
        /// </summary>
        public AppServiceScanOptions()
        {
            TransientTypeFinder = new TransientDependencyTypeFinder();
            ScopedTypeFinder = new ScopedDependencyTypeFinder();
            SingletonTypeFinder = new SingletonDependencyTypeFinder();
        }

        /// <summary>
        /// 获取或设置 即时生命周期服务类型查找器
        /// </summary>
        public ITypeFinder TransientTypeFinder { get; set; }

        /// <summary>
        /// 获取或设置 作用域生命周期服务类型查找器
        /// </summary>
        public ITypeFinder ScopedTypeFinder { get; set; }

        /// <summary>
        /// 获取或设置 单例生命周期服务类型查找器
        /// </summary>
        public ITypeFinder SingletonTypeFinder { get; set; }
    }
}