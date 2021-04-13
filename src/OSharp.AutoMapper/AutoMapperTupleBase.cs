// -----------------------------------------------------------------------
//  <copyright file="ProfileBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-07 18:29</last-date>
// -----------------------------------------------------------------------

using AutoMapper;

using OSharp.Mapping;


namespace OSharp.AutoMapper
{
    /// <summary>
    /// AutoMapper映射配置基类
    /// </summary>
    public abstract class AutoMapperTupleBase : Profile, IMapTuple
    {
        /// <summary>
        /// 获取 排序
        /// </summary>
        public virtual int Order => 0;

        /// <summary>
        /// 执行对象映射构造
        /// </summary>
        public abstract void CreateMap();
    }
}