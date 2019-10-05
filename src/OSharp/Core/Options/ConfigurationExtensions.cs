// -----------------------------------------------------------------------
//  <copyright file="ConfigurationExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2019 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2019-10-05 19:30</last-date>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Configuration;


namespace OSharp.Core.Options
{
    /// <summary>
    /// <see cref="IConfiguration"/>扩展方法
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// 从<see cref="IConfiguration"/>创建<see cref="OsharpOptions"/>
        /// </summary>
        public static OsharpOptions GetOsharpOptions(this IConfiguration configuration)
        {
            OsharpOptions options = new OsharpOptions();
            new OsharpOptionsSetup(configuration).Configure(options);
            return options;
        }
    }
}