// -----------------------------------------------------------------------
//  <copyright file="FunctionHandlerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-02-10 20:13</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OSharp.Collections;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Exceptions;
using OSharp.Reflection;


namespace OSharp.Authorization.Functions
{
    /// <summary>
    /// 功能信息处理基类
    /// </summary>
    public abstract class FunctionHandlerBase<TFunction> : IFunctionHandler
        where TFunction : class, IEntity<Guid>, IFunction, new()
    {
        private readonly List<TFunction> _functions = new List<TFunction>();
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化一个<see cref="FunctionHandlerBase{TFunction}"/>类型的新实例
        /// </summary>
        protected FunctionHandlerBase(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Logger = serviceProvider.GetLogger(GetType());
        }

        /// <summary>
        /// 获取 日志记录对象
        /// </summary>
        protected ILogger Logger { get; }
        
        /// <summary>
        /// 从程序集中获取功能信息（如MVC的Controller-Action）
        /// </summary>
        public void Initialize()
        {
            Type[] functionTypes = GetAllFunctionTypes();
            TFunction[] functions = GetFunctions(functionTypes);
            Logger.LogInformation($"功能信息初始化，共找到 {functions.Length} 个功能信息");

            _serviceProvider.ExecuteScopedWork(provider =>
            {
                SyncToDatabase(provider, functions);
            });

            RefreshCache();
        }

        /// <summary>
        /// 获取所有功能类型
        /// </summary>
        /// <returns></returns>
        public abstract Type[] GetAllFunctionTypes();

        /// <summary>
        /// 查找指定功能的所有功能点方法  
        /// </summary>
        /// <param name="functionType">功能类型</param>
        /// <returns></returns>
        public abstract MethodInfo[] GetMethodInfos(Type functionType);

        /// <summary>
        /// 查找指定条件的功能信息
        /// </summary>
        /// <param name="area">区域</param>
        /// <param name="controller">控制器</param>
        /// <param name="action">功能方法</param>
        /// <returns>功能信息</returns>
        public IFunction GetFunction(string area, string controller, string action)
        {
            if (_functions.Count == 0)
            {
                RefreshCache();
            }

            return _functions.FirstOrDefault(m =>
                string.Equals(m.Area, area, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Controller, controller, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Action, action, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 刷新功能信息缓存
        /// </summary>
        public void RefreshCache()
        {
            _serviceProvider.ExecuteScopedWork(provider =>
            {
                _functions.Clear();
                _functions.AddRange(GetFromDatabase(provider));
                Logger.LogInformation($"刷新功能信息缓存，从数据库获取到 {_functions.Count} 个功能信息");
            });
        }

        /// <summary>
        /// 清空功能信息缓存
        /// </summary>
        public void ClearCache()
        {
            _functions.Clear();
        }

        /// <summary>
        /// 从功能类型中获取功能信息
        /// </summary>
        /// <param name="functionTypes">功能类型集合</param>
        /// <returns></returns>
        protected virtual TFunction[] GetFunctions(Type[] functionTypes)
        {
            List<TFunction> functions = new List<TFunction>();
            foreach (Type type in functionTypes.OrderBy(m => m.FullName))
            {
                TFunction controller = GetFunction(type);
                if (controller == null || type.HasAttribute<NonFunctionAttribute>())
                {
                    continue;
                }

                if (!HasPickup(functions, controller))
                {
                    functions.Add(controller);
                    Logger.LogDebug($"提取功能信息：{controller}");
                }

                List<MethodInfo> methods = GetMethodInfos(type).ToList();
                // 移除已被重写的方法
                MethodInfo[] overriddenMethodInfos = methods.Where(m => m.IsOverridden()).ToArray();
                foreach (MethodInfo overriddenMethodInfo in overriddenMethodInfos)
                {
                    methods.RemoveAll(m => m.Name == overriddenMethodInfo.Name && m != overriddenMethodInfo);
                }

                foreach (MethodInfo method in methods)
                {
                    TFunction action = GetFunction(controller, method);
                    if (action == null)
                    {
                        continue;
                    }

                    if (IsIgnoreMethod(action, method, functions))
                    {
                        continue;
                    }

                    if (HasPickup(functions, action))
                    {
                        continue;
                    }

                    functions.Add(action);
                    Logger.LogDebug($"提取功能信息：{action}");
                }
            }

            return functions.ToArray();
        }

        /// <summary>
        /// 重写以实现从功能类型创建功能信息
        /// </summary>
        /// <param name="type">功能类型</param>
        /// <returns></returns>
        protected abstract TFunction GetFunction(Type type);

        /// <summary>
        /// 重写以实现从方法信息中创建功能信息
        /// </summary>
        /// <param name="typeFunction">类功能信息</param>
        /// <param name="method">方法信息</param>
        /// <returns></returns>
        protected abstract TFunction GetFunction(TFunction typeFunction, MethodInfo method);

        /// <summary>
        /// 重写以判断指定功能信息是否已提取过
        /// </summary>
        /// <param name="functions">已提取功能信息集合</param>
        /// <param name="function">要判断的功能信息</param>
        /// <returns></returns>
        protected virtual bool HasPickup(List<TFunction> functions, TFunction function)
        {
            return functions.Any(m =>
                string.Equals(m.Area, function.Area, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Controller, function.Controller, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Action, function.Action, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Name, function.Name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 重写以实现功能信息查找
        /// </summary>
        /// <param name="functions">功能信息集合</param>
        /// <param name="area">区域名称</param>
        /// <param name="controller">类型名称</param>
        /// <param name="action">方法名称</param>
        /// <param name="name">功能名称</param>
        /// <returns></returns>
        protected virtual TFunction GetFunction(IEnumerable<TFunction> functions, string area, string controller, string action, string name)
        {
            return functions.FirstOrDefault(m =>
                string.Equals(m.Area, area, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Controller, controller, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Action, action, StringComparison.OrdinalIgnoreCase)
                && string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 重写以实现是否忽略指定方法的功能信息
        /// </summary>
        /// <param name="action">要判断的功能信息</param>
        /// <param name="method">功能相关的方法信息</param>
        /// <param name="functions">已存在的功能信息集合</param>
        /// <returns></returns>
        protected virtual bool IsIgnoreMethod(TFunction action, MethodInfo method, IEnumerable<TFunction> functions)
        {
            if (method.HasAttribute<NonFunctionAttribute>())
            {
                return true;
            }
            TFunction existing = GetFunction(functions, action.Area, action.Controller, action.Action, action.Name);
            return existing != null;
        }

        /// <summary>
        /// 将从程序集获取的功能信息同步到数据库中
        /// </summary>
        /// <param name="scopedProvider">局部服务程序提供者</param>
        /// <param name="functions">程序集获取的功能信息</param>
        protected virtual void SyncToDatabase(IServiceProvider scopedProvider, TFunction[] functions)
        {
            Check.NotNull(functions, nameof(functions));
            if (functions.Length == 0)
            {
                return;
            }

            IRepository<TFunction, Guid> repository = scopedProvider.GetService<IRepository<TFunction, Guid>>();
            if (repository == null)
            {
                Logger.LogWarning("初始化功能数据时，IRepository<,>的服务未找到，请初始化 EntityFrameworkCoreModule 模块");
                return;
            }

            IUnitOfWork unitOfWork = scopedProvider.GetUnitOfWork(true);

            if (!functions.CheckSyncByHash(scopedProvider, Logger))
            {
                Logger.LogInformation("同步功能数据时，数据签名与上次相同，取消同步");
                return;
            }

            TFunction[] dbItems = repository.Query(null, false).ToArray();

            //删除的功能
            TFunction[] removeItems = dbItems.Except(functions,
                EqualityHelper<TFunction>.CreateComparer(m => m.Area + m.Controller + m.Action)).ToArray();
            int removeCount = removeItems.Length;
            //todo：由于外键关联不能物理删除的实体，需要实现逻辑删除
            foreach (TFunction function in removeItems)
            {
                repository.Delete(function);
                Logger.LogDebug($"删除功能信息：{function}");
            }

            //新增的功能
            TFunction[] addItems = functions.Except(dbItems,
                EqualityHelper<TFunction>.CreateComparer(m => m.Area + m.Controller + m.Action)).ToArray();
            int addCount = addItems.Length;
            foreach (TFunction function in addItems)
            {
                repository.Insert(function);
                Logger.LogDebug($"新增功能信息：{function}");
            }

            //更新的功能信息
            int updateCount = 0;
            foreach (TFunction item in dbItems.Except(removeItems))
            {
                bool isUpdate = false;
                TFunction function;
                try
                {
                    function = functions.Single(m =>
                        string.Equals(m.Area, item.Area, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(m.Controller, item.Controller, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(m.Action, item.Action, StringComparison.OrdinalIgnoreCase));
                }
                catch (InvalidOperationException)
                {
                    throw new OsharpException($"发现多个“{item.Area}-{item.Controller}-{item.Action}”的功能信息，不允许重名");
                }

                if (function == null)
                {
                    continue;
                }

                if (!string.Equals(item.Name, function.Name, StringComparison.OrdinalIgnoreCase))
                {
                    item.Name = function.Name;
                    isUpdate = true;
                }

                if (item.IsAjax != function.IsAjax)
                {
                    item.IsAjax = function.IsAjax;
                    isUpdate = true;
                }

                if (!item.IsAccessTypeChanged && item.AccessType != function.AccessType)
                {
                    item.AccessType = function.AccessType;
                    isUpdate = true;
                }

                if (isUpdate)
                {
                    repository.Update(item);
                    updateCount++;
                    Logger.LogDebug($"更新功能信息：{function}");
                }
            }

            unitOfWork.Commit();
            if (removeCount + addCount + updateCount > 0)
            {
                string msg = "刷新功能信息";
                if (addCount > 0)
                {
                    msg += "，添加功能信息 " + addCount + " 个";
                }

                if (updateCount > 0)
                {
                    msg += "，更新功能信息 " + updateCount + " 个";
                }

                if (removeCount > 0)
                {
                    msg += "，删除功能信息 " + removeCount + " 个";
                }

                Logger.LogInformation(msg);
            }
        }

        /// <summary>
        /// 从数据库获取最新功能信息
        /// </summary>
        /// <returns></returns>
        protected virtual TFunction[] GetFromDatabase(IServiceProvider scopedProvider)
        {
            IRepository<TFunction, Guid> repository = scopedProvider.GetService<IRepository<TFunction, Guid>>();
            if (repository == null)
            {
                return new TFunction[0];
            }

            TFunction[] functions = repository.QueryAsNoTracking(null, false).ToArray();
            return functions;
        }
    }
}