using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Extensions.Logging;

using OSharp.Core.Functions;
using OSharp.Entity;
using OSharp.Exceptions;
using OSharp.Reflection;


namespace OSharp.Security
{
    /// <summary>
    /// 模块信息处理器基类
    /// </summary>
    public abstract class ModuleHandlerBase<TModule, TModuleKey, TFunction, TModuleFunction> : IModuleHandler<TModule, TModuleKey>
        where TModule : ModuleBase<TModuleKey>
        where TModuleKey : struct, IEquatable<TModuleKey>
        where TFunction : class, IEntity<Guid>, IFunction, new()
        where TModuleFunction : ModuleFunctionBase<TModuleKey>
    {
        private readonly ServiceLocator _locator;

        /// <summary>
        /// 初始化一个<see cref="ModuleHandlerBase"/>类型的新实例
        /// </summary>
        protected ModuleHandlerBase()
        {
            _locator = ServiceLocator.Instance;
            Logger = _locator.GetService<ILoggerFactory>().CreateLogger(GetType());
            FunctionHandler = _locator.GetService<IFunctionHandler>();
        }

        /// <summary>
        /// 获取 日志记录对象
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// 获取 功能处理器
        /// </summary>
        protected IFunctionHandler FunctionHandler { get; }

        /// <summary>
        /// 从程序集中获取模块信息
        /// </summary>
        public void Initialize()
        {
            Check.NotNull(FunctionHandler, nameof(FunctionHandler));
            if (!(FunctionHandler is FunctionHandlerBase<TFunction> functionHandler))
            {
                throw new OsharpException($"属性“{nameof(FunctionHandler)}”必须实现基类“{typeof(FunctionHandlerBase<TFunction>)}”");
            }
            IFunctionTypeFinder functionTypeFinder = functionHandler.FunctionTypeFinder;
            Type[] functionTypes = functionTypeFinder.FindAll(true);
            ModuleInfo[] modules = GetModules(functionTypes, functionHandler);

            _locator.ExcuteScopedWork(provider =>
            {
                SyncToDatabase(provider, modules);
            });
        }

        /// <summary>
        /// 重写以实现从类型中提取模块信息
        /// </summary>
        /// <param name="types">待查找的类型</param>
        /// <param name="functionHandler">功能处理器</param>
        /// <returns>提取到的模块信息</returns>
        protected virtual ModuleInfo[] GetModules(Type[] types, FunctionHandlerBase<TFunction> functionHandler)
        {
            List<ModuleInfo> infos = new List<ModuleInfo>();
            foreach (Type type in types)
            {
                ModuleInfo info = GetModule(type);
                if (info == null)
                {
                    continue;
                }
                infos.Add(info);
                var all = functionHandler.MethodInfoFinder.FindAll(type);
                for (var index = 0; index < all.Length; index++)
                {
                    MethodInfo method = all[index];
                    info = GetModule(method, index);
                    if (info == null)
                    {
                        continue;
                    }
                    infos.Add(info);
                }
            }
            return infos.ToArray();
        }

        /// <summary>
        /// 重写以获取类型的模块信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>提取到的模块信息</returns>
        protected virtual ModuleInfo GetModule(Type type)
        {
            ModuleInfoAttribute infoAttr = type.GetAttribute<ModuleInfoAttribute>();
            if (infoAttr == null)
            {
                return null;
            }
            ModuleInfo info = new ModuleInfo()
            {
                Name = infoAttr.Name ?? type.GetDescription() ?? type.Name,
                Code = infoAttr.Code ?? type.Name,
                Order = infoAttr.Order,
                Position = GetPosition(type)
            };
            return info;
        }

        /// <summary>
        /// 重写以获取从方法中提取模块信息
        /// </summary>
        /// <param name="method">待查找的方法</param>
        /// <param name="index">序号</param>
        /// <returns>提取到的模块信息</returns>
        protected virtual ModuleInfo GetModule(MethodInfo method, int index)
        {
            ModuleInfoAttribute infoAttr = method.GetAttribute<ModuleInfoAttribute>();
            if (infoAttr == null)
            {
                return null;
            }
            ModuleInfo info = new ModuleInfo()
            {
                Name = infoAttr.Name ?? method.GetDescription() ?? method.Name,
                Code = infoAttr.Code ?? method.Name,
                Order = infoAttr.Order > 0 ? infoAttr.Order : index + 1,
                Position = GetPosition(method)
            };
            //依赖的功能

            return info;
        }

        /// <summary>
        /// 重写以实现从类型中获取模块位置字符串
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>以.拼接的位置字符串</returns>
        protected virtual string GetPosition(Type type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 重写以实现从方法中提取模块位置字符串
        /// </summary>
        /// <param name="method">方法</param>
        /// <returns>以.拼接的位置字符串</returns>
        protected virtual string GetPosition(MethodInfo method)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 重写以实现将从类库提取的模块信息同步到数据库中
        /// </summary>
        /// <param name="provider">区域服务提供者</param>
        /// <param name="modules">从类库中提取的模块信息，包含模块基本信息和模块依赖的功能信息</param>
        protected virtual void SyncToDatabase(IServiceProvider provider, ModuleInfo[] modules)
        {
            throw new NotImplementedException();
        }
    }
}
