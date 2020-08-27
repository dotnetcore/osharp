// -----------------------------------------------------------------------
//  <copyright file="DbContextModelCache.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-12 14:14</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Extensions;


namespace OSharp.Entity
{
    /// <summary>
    /// ����������ģ�ͻ���
    /// </summary>
    public class DbContextModelCache
    {
        private readonly ConcurrentDictionary<Type, IModel> _dict = new ConcurrentDictionary<Type, IModel>();
        private readonly ILogger _logger;

        /// <summary>
        /// ��ʼ��һ��<see cref="DbContextModelCache"/>���͵���ʵ��
        /// </summary>
        public DbContextModelCache(IServiceProvider provider)
        {
            _logger = provider.GetLogger(GetType());
        }

        /// <summary>
        /// ��ȡָ�����������͵�ģ��
        /// </summary>
        /// <param name="dbContextType">����������</param>
        /// <returns>����ģ��</returns>
        public IModel Get(Type dbContextType)
        {
            IModel model = _dict.GetOrDefault(dbContextType);
            _logger.LogDebug($"�� DbContextModelCache �л�ȡ���������� {dbContextType} ��Model���棬�����{model != null}");
            return model;
        }

        /// <summary>
        /// ����ָ�����������͵�ģ��
        /// </summary>
        /// <param name="dbContextType">����������</param>
        /// <param name="model">ģ��</param>
        public void Set(Type dbContextType, IModel model)
        {
            _logger.LogDebug($"�� DbContextModelCache �д������������� {dbContextType} ��Model����");
            _dict[dbContextType] = model;
        }

        /// <summary>
        /// �Ƴ�ָ�����������͵�ģ��
        /// </summary>
        public void Remove(Type dbContextType)
        {
            _logger.LogDebug($"�� DbContextModelCache ���Ƴ����������� {dbContextType} ��Model����");
            _dict.TryRemove(dbContextType, out IModel model);
        }
    }
}