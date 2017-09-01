// -----------------------------------------------------------------------
//  <copyright file="MapTupleBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-26 14:06</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.Mapping
{
    /// <summary>
    /// 对象映射源与目标配对基类
    /// </summary>
    /// <typeparam name="TSourceTypeFinder">源类型查找器类型</typeparam>
    /// <typeparam name="TTargetTypeFinder">目标类型查找器类型</typeparam>
    public abstract class MapTupleBase<TSourceTypeFinder, TTargetTypeFinder> : IMapTuple
        where TSourceTypeFinder : IMapSourceTypeFinder
        where TTargetTypeFinder : IMapTargetTypeFinder
    {
        /// <summary>
        /// 获取或设置 源类型查找器
        /// </summary>
        public TSourceTypeFinder SourceTypeFinder { get; set; }

        /// <summary>
        /// 获取或设置 目标类型查找器
        /// </summary>
        public TTargetTypeFinder TargetTypeFinder { get; set; }

        /// <summary>
        /// 执行对象映射构造
        /// </summary>
        public void Build()
        {
            Type[] sourceTypes = SourceTypeFinder.FindAll();
            Type[] targetTypes = TargetTypeFinder.FindAll();
            if (sourceTypes.Length == 0 || targetTypes.Length == 0)
            {
                return;
            }
            foreach (Type sourceType in sourceTypes)
            {
                foreach (Type targetType in targetTypes)
                {
                    if (IsMatch(sourceType, targetType))
                    {
                        CreateMapper(sourceType, targetType);
                        break;
                    }
                }
            }
        }
        
        /// <summary>
        /// 重写以定义源类型与目标类型的匹配规则
        /// </summary>
        /// <param name="sourceType">源类型</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        protected abstract bool IsMatch(Type sourceType, Type targetType);

        /// <summary>
        /// 重写以实现映射类型的创建
        /// </summary>
        /// <param name="sourceType">源类型</param>
        /// <param name="targetType">目标类型</param>
        protected abstract void CreateMapper(Type sourceType, Type targetType);
    }
}