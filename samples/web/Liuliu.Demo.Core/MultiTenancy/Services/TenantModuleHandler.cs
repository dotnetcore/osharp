// -----------------------------------------------------------------------
//  <copyright file="ModuleHandlerBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 18:21</last-date>
// -----------------------------------------------------------------------


using Liuliu.Demo.Authorization.Dtos;
using Liuliu.Demo.Authorization.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSharp.Authorization;
using OSharp.Authorization.Modules;
using OSharp.Collections;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Exceptions;

namespace Liuliu.Demo.MultiTenancy;

/// <summary>
/// 模块信息处理器
/// </summary>
public class TenantModuleHandler : IModuleHandler
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModuleInfoPicker _moduleInfoPicker;

    /// <summary>
    /// 初始化一个<see cref="TenantModuleHandler"/>类型的新实例
    /// </summary>
    public TenantModuleHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _moduleInfoPicker = serviceProvider.GetService<IModuleInfoPicker>();
        Logger = serviceProvider.GetLogger(GetType());
        ModuleInfos = Array.Empty<ModuleInfo>();
    }

    /// <summary>
    /// 获取 日志记录对象
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// 获取 所有模块信息
    /// </summary>
    public ModuleInfo[] ModuleInfos { get; private set; }

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
        Logger.LogInformation($"模块信息初始化，共找到 {moduleInfos.Length} 个模块Module信息");
        _serviceProvider.ExecuteScopedWork(provider =>
        {
            SyncToDatabase(provider, moduleInfos);
        });
        ModuleInfos = moduleInfos.OrderBy(m => $"{m.Position}.{m.Code}").ToArray();
    }
        
    /// <summary>
    /// 重写以实现将提取到的模块信息同步到数据库中
    /// </summary>
    /// <param name="provider">局部服务提供者</param>
    /// <param name="moduleInfos">从程序集中提取到的模块信息</param>
    protected virtual void SyncToDatabase(IServiceProvider provider, ModuleInfo[] moduleInfos)
    {
        _positionDictionary = new Dictionary<string, Module>();
        Check.NotNull(moduleInfos, nameof(moduleInfos));
        if (moduleInfos.Length == 0)
        {
            return;
        }

        IModuleStore<Module, ModuleInputDto, long> moduleStore =
            provider.GetService<IModuleStore<Module, ModuleInputDto, long>>();
        if (moduleStore == null)
        {
            Logger.LogWarning("初始化模块数据时，IRepository<,>的服务未找到，请初始化 EntityFrameworkCoreModule 模块");
            return;
        }

        IModuleFunctionStore<ModuleFunction, long> moduleFunctionStore =
            provider.GetService<IModuleFunctionStore<ModuleFunction, long>>();
        if (moduleFunctionStore == null)
        {
            Logger.LogWarning("初始化模块功能数据时，IRepository<,>的服务未找到，请初始化 EntityFrameworkCoreModule 模块");
            return;
        }

        IUnitOfWork unitOfWork = provider.GetUnitOfWork(true);

        if (!moduleInfos.CheckSyncByHash(provider, Logger) && moduleStore.Modules.Any())
        {
            Logger.LogInformation("同步模块数据时，数据签名与上次相同，取消同步");
            return;
        }

        //删除数据库中多余的模块
        Module[] modules = moduleStore.Modules.ToArray();
        var positionModules = modules.Select(m => new { m.Id, Position = GeModulePosition(modules, m) })
            .OrderByDescending(m => m.Position.Length).ToArray();
        string[] deletePositions = positionModules.Select(m => m.Position)
            .Except(moduleInfos.Select(n => $"{n.Position}.{n.Code}"))
            .Except(new[] { "Root" })
            .ToArray();
        long[] deleteModuleIds = positionModules.Where(m => deletePositions.Contains(m.Position)).Select(m => m.Id).ToArray();
        foreach (long id in deleteModuleIds)
        {
            OperationResult result = moduleStore.DeleteModule(id).GetAwaiter().GetResult();
            if (result.Error)
            {
                throw new OsharpException(result.Message);
            }
            Logger.LogDebug($"删除模块：{result.Message}");
        }

        //新增或更新传入的模块
        foreach (ModuleInfo info in moduleInfos)
        {
            Module parent = GeModule(moduleStore, info.Position);
            //插入父级分类
            if (parent == null)
            {
                int lastIndex = info.Position.LastIndexOf('.');
                string parent1Position = info.Position.Substring(0, lastIndex);
                Module parent1 = GeModule(moduleStore, parent1Position);
                if (parent1 == null)
                {
                    throw new OsharpException($"路径为“{parent1Position}”的模块信息无法找到");
                }
                string parentCode = info.Position.Substring(lastIndex + 1, info.Position.Length - lastIndex - 1);
                ModuleInfo parentInfo = new ModuleInfo() { Code = parentCode, Name = info.PositionName ?? parentCode, Position = parent1Position };
                ModuleInputDto dto = GetDto(parentInfo, parent1, null);
                OperationResult result = moduleStore.CreateModule(dto).GetAwaiter().GetResult();
                if (result.Error)
                {
                    throw new OsharpException(result.Message);
                }
                parent = moduleStore.Modules.First(m => m.ParentId.Equals(parent1.Id) && m.Code == parentCode);
            }
            Module module = moduleStore.Modules.FirstOrDefault(m => m.ParentId.Equals(parent.Id) && m.Code == info.Code);
            //新建模块
            if (module == null)
            {
                ModuleInputDto dto = GetDto(info, parent, null);
                OperationResult result = moduleStore.CreateModule(dto).GetAwaiter().GetResult();
                if (result.Error)
                {
                    throw new OsharpException(result.Message);
                }
                module = moduleStore.Modules.First(m => m.ParentId.Equals(parent.Id) && m.Code == info.Code);
            }
            else //更新模块
            {
                ModuleInputDto dto = GetDto(info, parent, module);
                OperationResult result = moduleStore.UpdateModule(dto).GetAwaiter().GetResult();
                if (result.Error)
                {
                    throw new OsharpException(result.Message);
                }
            }
            if (info.DependOnFunctions.Length > 0)
            {
                long[] functionIds = info.DependOnFunctions.Select(m => m.Id).ToArray();
                OperationResult result = moduleFunctionStore.SetModuleFunctions(module.Id, functionIds).GetAwaiter().GetResult();
                if (result.Error)
                {
                    throw new OsharpException(result.Message);
                }
            }
        }

        unitOfWork.Commit();
    }

    private IDictionary<string, Module> _positionDictionary = new Dictionary<string, Module>();
    private Module GeModule(IModuleStore<Module, ModuleInputDto, long> moduleStore, string position)
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
        Module module = moduleStore.Modules.FirstOrDefault(m => m.Code == code);
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

    private static ModuleInputDto GetDto(ModuleInfo info, Module parent, Module existsModule)
    {
        return new ModuleInputDto()
        {
            Id = existsModule?.Id ?? default(long),
            Name = info.Name,
            Code = info.Code,
            OrderCode = info.Order,
            Remark = $"{parent.Name}-{info.Name}",
            ParentId = parent.Id
        };
    }

    private static string GeModulePosition(Module[] source, Module module)
    {
        string[] codes = module.TreePathIds.Select(id => source.First(n => n.Id.Equals(id)).Code).ToArray();
        return codes.ExpandAndToString(".");
    }
}
