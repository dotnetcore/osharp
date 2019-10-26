// -----------------------------------------------------------------------
//  <copyright file="AppSettingsManager.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-03-19 22:49</last-date>
// -----------------------------------------------------------------------

using System.IO;

using Microsoft.Extensions.Configuration;

using OSharp.Extensions;


namespace OSharp.Core.Options
{
    /// <summary>
    /// appsettings配置文件读取器
    /// </summary>
    public static class AppSettingsReader
    {
        private static IConfiguration _configuration;

        static AppSettingsReader()
        {
            BuildConfiguration();
        }

        private static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
        }

        /// <summary>
        /// 读取指定节点信息
        /// </summary>
        /// <param name="key">节点名称，多节点以:分隔</param>
        public static string GetString(string key)
        {
            return _configuration[key];
        }

        /// <summary>
        /// 读取指定节点的简单数据类型的值
        /// </summary>
        /// <param name="key">节点名称，多节点以:分隔</param>
        /// <param name="defaultValue">默认值，读取失败时使用</param>
        public static T GetValue<T>(string key, T defaultValue = default)
        {
            string str = _configuration[key];
            return str.CastTo<T>(defaultValue);
        }

        /// <summary>
        /// 读取指定节点的复杂类型的值，并绑定到指定的空实例上
        /// </summary>
        /// <typeparam name="T">复杂类型</typeparam>
        /// <param name="key">节点名称，多节点以:分隔</param>
        /// <param name="instance">要绑定的空实例</param>
        /// <returns></returns>
        public static T GetInstance<T>(string key, T instance)
        {
            var config = _configuration.GetSection(key);
            if (!config.Exists())
            {
                return default(T);
            }
            config.Bind(instance);
            return instance;
        }
    }
}