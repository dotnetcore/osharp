// -----------------------------------------------------------------------
//  <copyright file="ModuleHandlerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 18:21</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Dependency;
using OSharp.Entity;
using OSharp.Exceptions;
using OSharp.Extensions;


namespace OSharp.Security
{
    /// <summary>
    /// 模块信息处理器基类
    /// </summary>
    public abstract class ModuleHandlerBase<TModule, TModuleInputDto, TModuleKey, TFunction, TModuleFunction> : IModuleHandler
        where TModule : ModuleBase<TModuleKey>
        where TModuleInputDto : ModuleInputDtoBase<TModuleKey>, new()
        where TModuleKey : struct, IEquatable<TModuleKey>
        where TFunction : class, IFunction, new()
        where TModuleFunction : ModuleFunctionBase<TModuleKey>
    {
        private readonly ServiceLocator _locator;
        private readonly IModuleInfoPicker _moduleInfoPicker;

        /// <summary>
        /// 初始化一个<see cref="ModuleHandlerBase"/>类型的新实例
        /// </summary>
        protected ModuleHandlerBase()
        {
            _locator = ServiceLocator.Instance;
            _moduleInfoPicker = _locator.GetService<IModuleInfoPicker>();
        }

        /// <summary>
        /// 从程序集中获取模块信息
        /// </summary>
        public void Initialize()
        {
            ModuleInfo[] moduleInfos = _moduleInfoPicker.Pickup();
            if (moduleInfos.Length == 0)
            {
                return;
            }
            _locator.ExcuteScopedWork(provider =>
            {
                SyncToDatabase(provider, moduleInfos);
            });
        }

        /// <summary>
        /// 重写以实现将提取到的模块信息同步到数据库中
        /// </summary>
        /// <param name="provider">局部服务提供者</param>
        /// <param name="moduleInfos">从程序集中提取到的模块信息</param>
        protected virtual void SyncToDatabase(IServiceProvider provider, ModuleInfo[] moduleInfos)
        {
            Check.NotNull(moduleInfos, nameof(moduleInfos));
            if (moduleInfos.Length == 0)
            {
                return;
            }
            IModuleStore<TModule, TModuleInputDto, TModuleKey> moduleStore =
                provider.GetService<IModuleStore<TModule, TModuleInputDto, TModuleKey>>();
            IModuleFunctionStore<TModuleFunction, TModuleKey> moduleFunctionStore =
                provider.GetService<IModuleFunctionStore<TModuleFunction, TModuleKey>>();

            foreach (ModuleInfo info in moduleInfos)
            {
                TModule parent = GetModule(moduleStore, info.Position);
                if (parent == null)
                {
                    throw new OsharpException($"路径为“{info.Position}”的模块信息无法找到");
                }
                OperationResult result;
                TModule module = moduleStore.Modules.FirstOrDefault(m => m.ParentId.Equals(parent.Id) && m.Code == info.Code);
                if (module == null)
                {
                    TModuleInputDto dto = GetDto(info, parent.Id, null);
                    result = moduleStore.CreateModule(dto).Result;
                    if (result.Errored)
                    {
                        throw new OsharpException(result.Message);
                    }
                    module = moduleStore.Modules.First(m => m.ParentId.Equals(parent.Id) && m.Code == info.Code);
                }
                else
                {
                    TModuleInputDto dto = GetDto(info, parent.Id, module);
                    result = moduleStore.UpdateModule(dto).Result;
                    if (result.Errored)
                    {
                        throw new OsharpException(result.Message);
                    }
                }
                if (info.DependOnFunctions.Length > 0)
                {
                    Guid[] functionIds = info.DependOnFunctions.Select(m => m.Id).ToArray();
                    result = moduleFunctionStore.SetModuleFunctions(module.Id, functionIds).Result;
                    if (result.Errored)
                    {
                        throw new OsharpException(result.Message);
                    }
                }
            }
            IUnitOfWork unitOfWork = provider.GetService<IUnitOfWork>();
            unitOfWork.Commit();
        }

        private readonly IDictionary<string, TModule> _positionDictionary = new Dictionary<string, TModule>();
        private TModule GetModule(IModuleStore<TModule, TModuleInputDto, TModuleKey> moduleStore, string position)
        {
            if (_positionDictionary.ContainsKey(position))
            {
                return _positionDictionary[position];
            }
            string[] codes = position.Split('.');
            if (codes.Length == 0)
            {
                return null;
            }
            string code = codes[0];
            TModule module = moduleStore.Modules.FirstOrDefault(m => m.Code == code);
            if (module == null)
            {
                return null;
            }
            for (int i = 1; i < codes.Length; i++)
            {
                code = codes[i];
                module = moduleStore.Modules.FirstOrDefault(m => m.Code == code && m.ParentId.Equals(module.Id));
                if (module == null)
                {
                    return null;
                }
            }
            _positionDictionary.Add(position, module);
            return module;
        }

        private static TModuleInputDto GetDto(ModuleInfo info, TModuleKey parentId, TModule existsModule)
        {
            return new TModuleInputDto()
            {
                Id = existsModule?.Id ?? default(TModuleKey),
                Name = info.Name,
                Code = info.Code,
                OrderCode = info.Order,
                Remark = existsModule?.Remark,
                ParentId = parentId
            };
        }
    }
}