// -----------------------------------------------------------------------
//  <copyright file="AboutViewModel.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-04-12 14:37</last-date>
// -----------------------------------------------------------------------

using System.Reflection;

using OSharp.Filter;
using OSharp.IO;
using OSharp.Reflection;
using OSharp.Wpf.Stylet;

using Stylet;


namespace OSharp.CodeGenerator.Views.Helps
{
    [Singleton]
    public class AboutViewModel : Screen
    {
        public AboutViewModel()
        {
            Version = Assembly.GetExecutingAssembly().GetProductVersion();
        }

        public bool IsShow { get; set; }

        public string Version { get; set; }
    }
}
