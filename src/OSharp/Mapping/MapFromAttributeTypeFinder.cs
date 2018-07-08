// -----------------------------------------------------------------------
//  <copyright file="MapFromAttributeTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-03 23:58</last-date>
// -----------------------------------------------------------------------

using OSharp.Reflection;


namespace OSharp.Mapping
{
    /// <summary>
    /// 标注了<see cref="MapFromAttribute"/>标签的类型查找器
    /// </summary>
    public class MapFromAttributeTypeFinder : AttributeTypeFinderBase<MapFromAttribute>, IMapFromAttributeTypeFinder
    {
        /// <summary>
        /// 初始化一个<see cref="MapFromAttributeTypeFinder"/>类型的新实例
        /// </summary>
        public MapFromAttributeTypeFinder(IAllAssemblyFinder allAssemblyFinder)
            : base(allAssemblyFinder)
        { }
    }
}