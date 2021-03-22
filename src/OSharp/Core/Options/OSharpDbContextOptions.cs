// -----------------------------------------------------------------------
//  <copyright file="DbContextConfig.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-09-03 0:54</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using OSharp.Data;
using OSharp.Entity;
using OSharp.Exceptions;


namespace OSharp.Core.Options
{
    /// <summary>
    /// 数据上下文配置节点
    /// </summary>
    public class OsharpDbContextOptions : DataErrorInfoBase
    {
        /// <summary>
        /// 初始化一个<see cref="OsharpDbContextOptions"/>类型的新实例
        /// </summary>
        public OsharpDbContextOptions()
        {
            LazyLoadingProxiesEnabled = false;
            AuditEntityEnabled = false;
            AutoMigrationEnabled = false;
        }

        /// <summary>
        /// 获取 上下文类型
        /// </summary>
        public Type DbContextType => string.IsNullOrEmpty(DbContextTypeName) ? null : Type.GetType(DbContextTypeName);

        /// <summary>
        /// 获取或设置 上下文类型全名
        /// </summary>
        [Required(ErrorMessage = "上下文类型全名不能为空")]
        public string DbContextTypeName { get; set; }

        /// <summary>
        /// 获取或设置 主数据库连接字符串
        /// </summary>
        [Required(ErrorMessage = "数据库连接串不能为空")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 获取或设置 数据库类型
        /// </summary>
        public DatabaseType DatabaseType { get; set; }

        /// <summary>
        /// 获取或设置 从数据库选择策略名
        /// </summary>
        public string SlaveSelectorName { get; set; }

        /// <summary>
        /// 获取或设置 从数据库选项集合
        /// </summary>
        public SlaveDatabaseOptions[] Slaves { get; set; }

        /// <summary>
        /// 获取或设置 是否启用延迟加载代理
        /// </summary>
        public bool LazyLoadingProxiesEnabled { get; set; }

        /// <summary>
        /// 获取或设置 是否允许审计实体
        /// </summary>
        public bool AuditEntityEnabled { get; set; }

        /// <summary>
        /// 获取或设置 是否自动迁移
        /// </summary>
        public bool AutoMigrationEnabled { get; set; }

        /// <summary>获取一条错误消息，指示此对象有什么问题。</summary>
        /// <returns>指示此对象存在什么问题的错误消息。默认值为空字符串（""）。</returns>
        public override string Error
        {
            get
            {
                string[] props = { "DbContextTypeName", "ConnectionString" };
                foreach (string prop in props)
                {
                    string msg = this[prop];
                    if (msg != string.Empty)
                    {
                        return $"属性{prop}验证失败：{msg}";
                    }
                }

                if (DbContextType == null)
                {
                    return $"属性DbContextTypeName提供的类型 {DbContextTypeName} 不存在";
                }

                if (Slaves != null)
                {
                    foreach (var slaveDatabase in Slaves)
                    {
                        string msg = slaveDatabase.Error;
                        if (msg != string.Empty)
                        {
                            return msg;
                        }
                    }
                }

                return string.Empty;
            }
        }
    }
}