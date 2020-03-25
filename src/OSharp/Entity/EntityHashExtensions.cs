// -----------------------------------------------------------------------
//  <copyright file="EntityHashExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-08-12 10:09</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Collections;
using OSharp.Core.Data;
using OSharp.Core.Systems;
using OSharp.Extensions;


namespace OSharp.Entity
{
    /// <summary>
    /// 实体Hash扩展方法
    /// </summary>
    public static class EntityHashExtensions
    {
        /// <summary>
        /// 检查指定实体的Hash值，决定是否需要进行数据库同步
        /// </summary>
        public static bool CheckSyncByHash(this IEnumerable<IEntityHash> entityHashes, IServiceProvider provider, ILogger logger)
        {
            IEntityHash[] hashes = entityHashes as IEntityHash[] ?? entityHashes.ToArray();
            if (hashes.Length == 0)
            {
                return false;
            }
            string hash = hashes.Select(m => m.GetHash()).ExpandAndToString().ToMd5Hash();
            IKeyValueStore store = provider.GetService<IKeyValueStore>();
            string entityType = hashes[0].GetType().FullName;
            string key = $"OSharp.Initialize.SyncToDatabaseHash-{entityType}";
            IKeyValue keyValue = store.GetKeyValue(key);
            if (keyValue != null && keyValue.Value?.ToString() == hash)
            {
                logger.LogInformation($"{hashes.Length}条基础数据“{entityType}”的内容签名 {hash} 与上次相同，取消数据库同步");
                return false;
            }

            store.CreateOrUpdateKeyValue(key, hash).GetAwaiter().GetResult();
            logger.LogInformation($"{hashes.Length}条基础数据“{entityType}”的内容签名 {hash} 与上次 {keyValue?.Value} 不同，将进行数据库同步");
            return true;
        }

        /// <summary>
        /// 获取指定实体的Hash值
        /// </summary>
        /// <param name="entity">实体对象</param>
        public static string GetHash(this IEntityHash entity)
        {
            Type type = entity.GetType();
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(m => m.CanWrite && m.Name != "Id"))
            {
                sb.Append(property.GetValue(entity));
            }
            return sb.ToString().ToMd5Hash();
        }
    }
}