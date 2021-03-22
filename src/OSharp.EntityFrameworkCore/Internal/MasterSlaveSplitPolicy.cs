// -----------------------------------------------------------------------
//  <copyright file="MasterSlaveSplitPolicy.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2021 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2021-03-21 12:35</last-date>
// -----------------------------------------------------------------------

using System;

using Castle.Core.Internal;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Authorization.Functions;
using OSharp.Core.Options;
using OSharp.Dependency;


namespace OSharp.Entity.Internal
{
    /// <summary>
    /// 主从分离策略
    /// </summary>
    internal class MasterSlaveSplitPolicy : IMasterSlaveSplitPolicy
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ScopedDictionary _scopedDict;

        /// <summary>
        /// 初始化一个<see cref="MasterSlaveSplitPolicy"/>类型的新实例
        /// </summary>
        public MasterSlaveSplitPolicy(IServiceProvider provider)
        {
            _unitOfWork = provider.GetUnitOfWork(false);
            _scopedDict = provider.GetRequiredService<ScopedDictionary>();
        }

        /// <summary>
        /// 是否前往从数据库
        /// </summary>
        /// <param name="options">数据上下文选项</param>
        /// <returns></returns>
        public bool IsToSlaveDatabase(OsharpDbContextOptions options)
        {
            SlaveDatabaseOptions[] slaves = options.Slaves;
            if (slaves.IsNullOrEmpty())
            {
                return false;
            }

            //允许工作单元事务，走主库
            if (_unitOfWork.IsEnabledTransaction)
            {
                return false;
            }

            // 在Function显式配置走从库，才走从库
            IFunction function = _scopedDict.Function;
            if (function == null || !function.IsSlaveDatabase)
            {
                return false;
            }

            return true;
        }
    }
}