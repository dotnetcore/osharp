// -----------------------------------------------------------------------
//  <copyright file="ServiceScanOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-29 1:45</last-date>
// -----------------------------------------------------------------------

using OSharp.Reflection;


namespace OSharp.Dependency
{
    /// <summary>
    /// 依赖注入服务类型扫描配置信息
    /// </summary>
    public class ServiceScanOptions
    {
        /// <summary>
        /// 初始化一个<see cref="ServiceScanOptions"/>类型的新实例
        /// </summary>
        public ServiceScanOptions()
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