// -----------------------------------------------------------------------
//  <copyright file="ConnectionStringProvider.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-20 21:42</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Exceptions;


namespace OSharp.Entity
{
    /// <summary>
    /// 数据库连接字符串提供者
    /// </summary>
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IDictionary<string, OsharpDbContextOptions> _dbContexts;
        private readonly ISlaveDatabaseSelector[] _slaveDatabaseSelectors;
        private readonly IMasterSlaveSplitPolicy _masterSlavePolicy;

        /// <summary>
        /// 初始化一个<see cref="ConnectionStringProvider"/>类型的新实例
        /// </summary>
        public ConnectionStringProvider(IServiceProvider provider)
        {
            _dbContexts = provider.GetOSharpOptions().DbContexts;
            _masterSlavePolicy = provider.GetService<IMasterSlaveSplitPolicy>();
            _slaveDatabaseSelectors = provider.GetServices<ISlaveDatabaseSelector>().ToArray();
        }

        /// <summary>
        /// 获取指定数据上下文类型的数据库连接字符串
        /// </summary>
        /// <param name="dbContextType">数据上下文类型</param>
        /// <returns></returns>
        public virtual string GetConnectionString(Type dbContextType)
        {
            OsharpDbContextOptions dbContextOptions = _dbContexts.Values.FirstOrDefault(m => m.DbContextType == dbContextType);
            if (dbContextOptions == null)
            {
                throw new OsharpException($"数据上下文“{dbContextType}”的数据上下文配置信息不存在");
            }

            bool isSlave = _masterSlavePolicy.IsToSlaveDatabase(dbContextOptions);
            if (!isSlave)
            {
                return dbContextOptions.ConnectionString;
            }
            
            SlaveDatabaseOptions[] slaves = dbContextOptions.Slaves;
            ISlaveDatabaseSelector slaveDatabaseSelector = _slaveDatabaseSelectors.LastOrDefault(m => m.Name == dbContextOptions.SlaveSelectorName)
                ?? _slaveDatabaseSelectors.First(m => m.Name == "Weight");
            SlaveDatabaseOptions slave = slaveDatabaseSelector.Select(slaves);
            return slave.ConnectionString;
        }
    }
}