// -----------------------------------------------------------------------
//  <copyright file="ModuleSeedDataInitializer.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2020 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2020-03-07 1:01</last-date>
// -----------------------------------------------------------------------

using OSharp.Data.Snows;
using OSharp.Hosting.Authorization.Entities;


namespace OSharp.Hosting.Authorization;

public class ModuleSeedDataInitializer : SeedDataInitializerBase<Module, long>
{
    /// <summary>
    /// 初始化一个<see cref="SeedDataInitializerBase{TEntity, TKey}"/>类型的新实例
    /// </summary>
    public ModuleSeedDataInitializer(IServiceProvider rootProvider)
        : base(rootProvider)
    { }

    /// <summary>
    /// 重写以提供要初始化的种子数据
    /// </summary>
    /// <returns></returns>
    protected override Module[] SeedData(IServiceProvider provider)
    {
        long id = IdHelper.NextId();
        return new[]
        {
            new Module() {Id = id, Name = "根节点", Remark = "系统根节点", Code = "Root", OrderCode = 1, TreePathString = $"${id}$" },
        };
    }

    /// <summary>
    /// 重写以提供判断某个实体是否存在的表达式
    /// </summary>
    /// <param name="entity">要判断的实体</param>
    /// <returns></returns>
    protected override Expression<Func<Module, bool>> ExistingExpression(Module entity)
    {
        return m => m.TreePathString == entity.TreePathString;
    }
}
