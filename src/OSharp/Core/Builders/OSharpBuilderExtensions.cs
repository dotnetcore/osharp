// -----------------------------------------------------------------------
//  <copyright file="OSharpBuilderExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-02 14:27</last-date>
// -----------------------------------------------------------------------

using OSharp.Core.Packs;


namespace OSharp.Core.Builders
{
    /// <summary>
    /// IOSharpBuilder扩展方法
    /// </summary>
    public static class OSharpBuilderExtensions
    {
        /// <summary>
        /// 添加CorePack
        /// </summary>
        public static IOSharpBuilder AddCorePack(this IOSharpBuilder builder)
        {
            return builder.AddPack<OSharpCorePack>();
        }
    }
}