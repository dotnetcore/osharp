// -----------------------------------------------------------------------
//  <copyright file="MultiTenancyPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-03 0:45</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.AspNetCore;
using OSharp.Core.Packs;


namespace OSharp.MultiTenancy
{
    /// <summary>
    /// 多租户模块基类
    /// </summary>
    [DependsOnPacks(typeof(AspNetCorePack))]
    public abstract class MultiTenancyPackBase : OsharpPack
    { }
}