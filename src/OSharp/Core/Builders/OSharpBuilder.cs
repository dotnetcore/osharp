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
            Modules = new List<Type>();
        }

        /// <summary>
        /// 获取 加载的模块集合
        /// </summary>
        public IEnumerable<Type> Modules { get; private set; }

        /// <summary>
        /// 获取 OSharp选项配置委托
        /// </summary>
        public Action<OSharpOptions> OptionsAction { get; private set; }

        /// <summary>
        /// 添加指定模块
        /// </summary>
        /// <typeparam name="TModule">要添加的模块类型</typeparam>
        public IOSharpBuilder AddModule<TModule>() where TModule : OSharpModule
        {
            List<Type> list = Modules.ToList();
            list.AddIfNotExist(typeof(TModule));
            Modules = list;
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
