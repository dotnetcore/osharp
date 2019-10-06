// -----------------------------------------------------------------------
//  <copyright file="AspNetCoreMvcPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-29 12:15</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;

using OSharp.AspNetCore.Mvc;


namespace Liuliu.Demo.Web.Startups
{
    /// <summary>
    /// MVC模块，此模块需要在Identity之后启动
    /// </summary>
    [Description("MVC模块")]
    public class AspNetCoreMvcPack : MvcPackBase
    {
        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 1;
    }
}