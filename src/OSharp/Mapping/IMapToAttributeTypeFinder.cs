// -----------------------------------------------------------------------
//  <copyright file="IMapToTypeFinder.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-13 23:52</last-date>
// -----------------------------------------------------------------------

using OSharp.Reflection;


namespace OSharp.Mapping
{
    /// <summary>
    /// 标注了<see cref="MapToAttribute"/>标签的类型查找器
    /// </summary>
    public interface IMapToAttributeTypeFinder : ITypeFinder
    { }
}