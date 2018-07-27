// -----------------------------------------------------------------------
//  <copyright file="UnitOfWork.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-21 22:20</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Options;
using OSharp.Entity.Transactions;
using OSharp.Exceptions;


namespace OSharp.Entity
{
    /// <summary>
    /// 业务单元操作
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DbContextGroupManager _groupManager;

        /// <summary>
        /// 初始化一个<see cref="UnitOfWork"/>类型的新实例
        /// </summary>
        public UnitOfWork(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            HasCommited = false;
            _groupManager = new DbContextGroupManager();
        }

        /// <summary>
        /// 获取 事务是否已提交
        /// </summary>
        public bool HasCommited { get; private set; }

        /// <summary>
        /// 获取指定数据上下文类型<typeparamref name="TEntity"/>的实例，并将同数据库连接字符串的上下文实例进行分组归类
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <returns><typeparamref name="TEntity"/>所属上下文类的实例</returns>
        public IDbContext GetDbContext<TEntity, TKey>() where TEntity : IEntity<TKey> where TKey : IEquatable<TKey>
        {
            IEntityConfigurationTypeFinder typeFinder = _serviceProvider.GetService<IEntityConfigurationTypeFinder>();
            Type entityType = typeof(TEntity);
            Type dbContextType = typeFinder.GetDbContextTypeForEntity(entityType);

            DbContextBase dbContext;
            OSharpDbContextOptions dbContextOptions = GetDbContextResolveOptions(dbContextType);
            DbContextResolveOptions resolveOptions = new DbContextResolveOptions(dbContextOptions);
            IDbContextResolver contextResolver = _serviceProvider.GetService<IDbContextResolver>();
            DbContextGroup group = _groupManager.Get(resolveOptions.ConnectionString);
            //连接字符串的上下文组不存在，创建组
            if (group == null)
            {
                resolveOptions.ExistingConnection = null;
                dbContext = (DbContextBase)contextResolver.Resolve(resolveOptions);
                if (!dbContext.ExistsRelationalDatabase())
                {
                    throw new OsharpException($"数据上下文“{dbContext.GetType().FullName}”的数据库不存在，请通过 Migration 功能进行数据迁移创建数据库。");
                }
                group = new DbContextGroup();
                group.DbContexts.Add(dbContext);
                dbContext.ContextGroup = group;
                _groupManager.Set(resolveOptions.ConnectionString, group);
            }
            else
            {
                resolveOptions.ExistingConnection = group.DbContexts[0].Database.GetDbConnection();
                //相同连接串相同上下文类型并且已存在对象，直接返回上下文对象
                dbContext = group.DbContexts.FirstOrDefault(m => m.GetType() == resolveOptions.DbContextType);
                if (dbContext != null)
                {
                    return dbContext;
                }
                dbContext = (DbContextBase)contextResolver.Resolve(resolveOptions);
                group.DbContexts.Add(dbContext);
                dbContext.ContextGroup = group;
            }
            return dbContext;
        }

        /// <summary>
        /// 对指定数据上下文开启或使用已存在事务
        /// </summary>
        /// <param name="context">上下文</param>
        public void BeginOrUseTransaction(IDbContext context)
        {
            if (!(context is DbContext dbContext))
            {
                return;
            }
            _groupManager.BeginOrUseTransaction(dbContext);
        }

        /// <summary>
        /// 异步对指定数据上下文开启或使用已存在事务
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="cancellationToken">异步取消标记</param>
        public async Task BeginOrUseTransactionAsync(IDbContext context, CancellationToken cancellationToken)
        {
            if (!(context is DbContext dbContext))
            {
                return;
            }
            await _groupManager.BeginOrUseTransactionAsync(dbContext, cancellationToken);
        }

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        public void Commit()
        {
            if (HasCommited)
            {
                return;
            }
            _groupManager.Commit();
            HasCommited = true;
        }

        private OSharpDbContextOptions GetDbContextResolveOptions(Type dbContextType)
        {
            OSharpDbContextOptions dbContextOptions = _serviceProvider.GetOSharpOptions()?.GetDbContextOptions(dbContextType);
            if (dbContextOptions == null)
            {
                throw new OsharpException($"无法找到数据上下文“{dbContextType}”的配置信息");
            }
            return dbContextOptions;
        }

        /// <summary>释放对象.</summary>
        public void Dispose()
        {
            _groupManager.Dispose();
        }
    }
}