using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OSharp.Collections;
using OSharp.Core.Modules;
using OSharp.Core.Options;


namespace OSharp.Core.Builders
{
    /// <summary>
    /// OSharp构建器
    /// </summary>
    public class OSharpBuilder : IOSharpBuilder
    {
        /// <summary>
        /// 初始化一个<see cref="OSharpBuilder"/>类型的新实例
        /// </summary>
        public OSharpBuilder()
        {
            AddModules = new List<Type>();
            ExceptModules = new List<Type>();
        }

        /// <summary>
        /// 获取 加载的模块集合
        /// </summary>
        public IEnumerable<Type> AddModules { get; private set; }

        /// <summary>
        /// 获取 排除的模块集合
        /// </summary>
        public IEnumerable<Type> ExceptModules { get; private set; }

        /// <summary>
        /// 获取 OSharp选项配置委托
        /// </summary>
        public Action<OSharpOptions> OptionsAction { get; private set; }

        /// <summary>
        /// 添加指定模块，执行此功能后将仅加载指定的模块
        /// </summary>
        /// <typeparam name="TModule">要添加的模块类型</typeparam>
        public IOSharpBuilder AddModule<TModule>() where TModule : OSharpModule
        {
            List<Type> list = AddModules.ToList();
            list.AddIfNotExist(typeof(TModule));
            AddModules = list;
            return this;
        }

        /// <summary>
        /// 移除指定模块，执行此功能以从自动加载的模块中排除指定模块
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <returns></returns>
        public IOSharpBuilder ExceptModule<TModule>() where TModule : OSharpModule
        {
            List<Type> list = ExceptModules.ToList();
            list.AddIfNotExist(typeof(TModule));
            ExceptModules = list;
            return this;
        }

        /// <summary>
        /// 添加OSharp选项配置
        /// </summary>
        /// <param name="optionsAction">OSharp操作选项</param>
        /// <returns>OSharp构建器</returns>
        public IOSharpBuilder AddOptions(Action<OSharpOptions> optionsAction)
        {
            Check.NotNull(optionsAction, nameof(optionsAction));
            OptionsAction = optionsAction;
            return this;
        }
    }
}
