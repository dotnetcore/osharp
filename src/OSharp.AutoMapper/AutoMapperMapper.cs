// -----------------------------------------------------------------------
//  <copyright file="AutoMapperMapper.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-01 12:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using IMapper = OSharp.Mapping.IMapper;


namespace OSharp.AutoMapper
{
    /// <summary>
    /// AutoMapper映射执行类
    /// </summary>
    public class AutoMapperMapper : IMapper
    {
        private readonly MapperConfiguration _configuration;
        private readonly global::AutoMapper.IMapper _mapper;

        /// <summary>
        /// 初始化一个<see cref="AutoMapperMapper"/>类型的新实例
        /// </summary>
        public AutoMapperMapper(MapperConfiguration configuration)
        {
            _configuration = configuration;
            _mapper = configuration.CreateMapper();
        }

        /// <summary>
        /// 将对象映射为指定类型
        /// </summary>
        /// <typeparam name="TTarget">要映射的目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <returns>目标类型的对象</returns>
        public TTarget MapTo<TTarget>(object source)
        {
            return _mapper.Map<TTarget>(source);
        }

        /// <summary>
        /// 使用源类型的对象更新目标类型的对象
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="target">待更新的目标对象</param>
        /// <returns>更新后的目标类型对象</returns>
        public TTarget MapTo<TSource, TTarget>(TSource source, TTarget target)
        {
            return _mapper.Map<TSource, TTarget>(source, target);
        }

        /// <summary>
        /// 将数据源映射为指定输出DTO的集合
        /// </summary>
        /// <typeparam name="TOutputDto">输出DTO类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="membersToExpand">成员展开</param>
        /// <returns>输出DTO的结果集</returns>
        public IQueryable<TOutputDto> ToOutput<TOutputDto>(IQueryable source, params Expression<Func<TOutputDto, object>>[] membersToExpand)
        {
            return source.ProjectTo(_configuration, membersToExpand);
        }
    }
}