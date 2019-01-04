// -----------------------------------------------------------------------
//  <copyright file="AuditPackBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-02 15:16</last-date>
// -----------------------------------------------------------------------



using OSharp.Core.Packs;
using OSharp.EventBuses;


namespace OSharp.Audits
{
    /// <summary>
    /// 审计模块基类
    /// </summary>
    [DependsOnPacks(typeof(EventBusPack))]
    public abstract class AuditPackBase : OsharpPack
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Application;
    }
}