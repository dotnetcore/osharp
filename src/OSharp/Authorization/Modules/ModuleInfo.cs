// -----------------------------------------------------------------------
//  <copyright file="ModuleInfo.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-10 20:13</last-date>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics;

using Microsoft.DotNet.PlatformAbstractions;

using OSharp.Authorization.Functions;
using OSharp.Entity;


namespace OSharp.Authorization.Modules
{
    /// <summary>
    /// 从程序集中提取的模块信息载体，包含模块基本信息和模块依赖的功能信息集合
    /// </summary>
    [DebuggerDisplay("{ToDebugDisplay()}")]
    public class ModuleInfo : IEntityHash
    {
        /// <summary>
        /// 获取或设置 模块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 模块代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 获取或设置 层次序号
        /// </summary>
        public double Order { get; set; }

        /// <summary>
        /// 获取或设置 模块位置，父模块Code以点号 . 相连的字符串
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 获取或设置 父级模块名称，需要创建父级模块的时候设置值
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// 获取或设置 依赖功能
        /// </summary>
        public IFunction[] DependOnFunctions { get; set; } = new IFunction[0];

        private string ToDebugDisplay()
        {
            return $"{Name}[{Code}]({Position}),FunctionCount:{DependOnFunctions.Length}";
        }

        #region Overrides of Object

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ModuleInfo info))
            {
                return false;
            }

            return $"{info.Position}.{info.Code}" == $"{Position}.{Code}";
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
#if NET5_0
            return HashCode.Combine(Position, Code);
#else
            var combiner = new HashCodeCombiner();
            combiner.Add(Position);
            combiner.Add(Code);
            return combiner.CombinedHash;
#endif
        }

        #endregion
    }
}