// -----------------------------------------------------------------------
//  <copyright file="OSharpPackTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-09 22:22</last-date>
// -----------------------------------------------------------------------

using OSharp.Reflection;


namespace OSharp.Core.Packs
{
    /// <summary>
    /// OSharp模块类型查找器
    /// </summary>
    public class OsharpPackTypeFinder : BaseTypeFinderBase<OsharpPack>, ITypeFinder
    {
        /// <summary>
        /// 初始化一个<see cref="OsharpPackTypeFinder"/>类型的新实例
        /// </summary>
        public OsharpPackTypeFinder(IAllAssemblyFinder allAssemblyFinder)
            : base(allAssemblyFinder)
        { }
    }
}