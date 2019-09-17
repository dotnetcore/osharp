// -----------------------------------------------------------------------
// <once-generated>
//     这个文件只生成一次，再次生成不会被覆盖。
//     可以在此类进行继承重写来扩展基类 InfosServiceBase
// </once-generated>
//
//  <copyright file="IInfosService.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>https://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
// -----------------------------------------------------------------------

using System;


namespace Liuliu.Demo.Infos
{
    /// <summary>
    /// 业务实现基类：信息模块
    /// </summary>
    public partial class InfosService : InfosServiceBase
    {
        /// <summary>
        /// 初始化一个<see cref="InfosService"/>类型的新实例
        /// </summary>
        public InfosService(IServiceProvider provider)
            : base(provider)
        { }
    }
}
