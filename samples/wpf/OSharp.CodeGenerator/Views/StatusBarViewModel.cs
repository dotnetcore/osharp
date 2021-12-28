// -----------------------------------------------------------------------
//  <copyright file="StatusBarViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-05-04 11:32</last-date>
// -----------------------------------------------------------------------

using OSharp.Wpf.Stylet;

using Stylet;


namespace OSharp.CodeGenerator.Views
{
    [Singleton]
    public class StatusBarViewModel : Screen
    {
        /// <summary>
        /// 获取或设置 状态栏消息
        /// </summary>
        public string Message { get; set; }
    }
}
