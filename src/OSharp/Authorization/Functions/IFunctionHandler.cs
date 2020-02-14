// -----------------------------------------------------------------------
//  <copyright file="IFunctionHandler.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-10 20:14</last-date>
// -----------------------------------------------------------------------

using OSharp.Reflection;


namespace OSharp.Authorization.Functions
{
    /// <summary>
    /// 定义功能信息处理器
    /// </summary>
    public interface IFunctionHandler
    {
        /// <summary>
        /// 获取 功能类型查找器
        /// </summary>
        IFunctionTypeFinder FunctionTypeFinder { get; }

        /// <summary>
        /// 获取 功能方法查找器
        /// </summary>
        IMethodInfoFinder MethodInfoFinder { get; }

        /// <summary>
        /// 从程序集中获取功能信息（如MVC的Controller-Action）
        /// </summary>
        void Initialize();

        /// <summary>
        /// 查找指定条件的功能信息
        /// </summary>
        /// <param name="area">区域</param>
        /// <param name="controller">控制器</param>
        /// <param name="action">功能方法</param>
        /// <returns>功能信息</returns>
        IFunction GetFunction(string area, string controller, string action);

        /// <summary>
        /// 刷新功能信息缓存
        /// </summary>
        void RefreshCache();

        /// <summary>
        /// 清空功能信息缓存
        /// </summary>
        void ClearCache();
    }
}